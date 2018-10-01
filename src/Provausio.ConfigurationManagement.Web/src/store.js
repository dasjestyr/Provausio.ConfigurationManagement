import Vue from 'vue'
import Vuex from 'vuex'
import AppService from './services/ApplicationService'
import xid from 'xid-js'

Vue.use(Vuex)

const appService = new AppService()
const defaultApp = { activeTab: 0, app: {} }

function getNewTab() {
  return {
      id: xid.next(),
      name: '+ Add',
      format: 'javascript',
      configuration: '// add configuration here',
      metadata: { isNew: true, canEditName: true }
  }
}

export default new Vuex.Store({
  state: {
    applications: {},
    activeApplication: defaultApp
  },  
  mutations: {    
    SET_APPLICATIONS: (state, payload) => {
      state.applications = payload
    },
    SET_ACTIVE_APPLICATION: (state, payload) => {      
      state.activeApplication = payload
    },
    CLEAR_ACTIVE_APPLICATION: (state) => {
      state.activeApplication = defaultApp
    },
    ADD_APPLICATION: (state, payload) => {
      state.applications[payload.id] = payload
    },
    ADD_ENVIRONMENT: (state, payload) => {
      state.activeApplication.app.environments.splice(0, 0, payload)
    },
    SET_ENVIRONMENTS: (state, environments) => {         
      state.activeApplication.app.environments = environments
    },
    DELETE_ENVIRONMENT: (state, environmentName) => {
      state.activeApplication.app.environments =
        state.activeApplication.app.environments.filter(e => {
          if(e.name !== environmentName)
            return e
        })
        state.activeApplication.activeTab = 0
    }
  },

  getters: {
    getApplications: state => {
      // because they're stored as objects instead of arrays
      return Object.keys(state.applications)
        .map(k => state.applications[k] || {})
    },
    getEnvironments: (state) => {     
      return state.activeApplication.app.environments
    },
    getActiveApplication: state =>  state.activeApplication
  },

  actions: {
    setApplications: (context, applications) => {      
      context.commit('SET_APPLICATIONS', applications)
    },
    getApplication: async (context, applicationId) => {
      let application = await appService.getApplication(applicationId)
      let active = { activeTab: 0, app: application }
      context.commit('SET_ACTIVE_APPLICATION', active)      
    },
    clearApplication: (context) => {
      context.commit('CLEAR_ACTIVE_APPLICATION')
    },
    getApplicationsFromServer: async (context) => {
      let applications = await appService.getApplications()    
      if(applications) context.commit('SET_APPLICATIONS', applications)
    },
    addApplication: async (context, payload) => {
      payload.id = await appService.createApplication(payload)
      context.commit('ADD_APPLICATION', payload)
    },
    addingEnvironment: (context) => {
      context.commit('ADD_ENVIRONMENT', getNewTab())
    },
    addEnvironment: (context, payload) => {
      // TODO: save to server
      context.commit('ADD_ENVIRONMENT', payload)
    },
    getEnvironments: async (context) => {
      let appId = context.getters.getActiveApplication.app.id
      let environments = await appService.getEnvironments(appId)
      
      environments.push(getNewTab())

      context.commit('SET_ENVIRONMENTS', environments)
    },
    deleteEnvironment: async (context, environmentName) => {
      let appId = context.getters.getActiveApplication.app.id
      await appService.deleteEnvironment(appId, environmentName)
      context.commit('DELETE_ENVIRONMENT', environmentName)
    }
  }
})
