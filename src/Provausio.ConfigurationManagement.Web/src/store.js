import Vue from 'vue'
import Vuex from 'vuex'
import AppService from './services/ApplicationService'

Vue.use(Vuex)

const appService = new AppService()
const defaultApp = { activeTab: 0, app: {} }

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
      state.applications.push(payload)
    },
    ADD_ENVIRONMENT: (state, appId, payload) => {
      state.applications[appId].environments.push(payload)
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
    addEnvironment: (context, appId, payload) => {
      context.commit('ADD_ENVIRONMENT', appId, payload)
    },
    getEnvironments: async (context, staticTabs) => {
      let appId = context.getters.getActiveApplication.app.id
      let environments = await appService.getEnvironments(appId)
      
      if(staticTabs)
        environments.push(...staticTabs)

      context.commit('SET_ENVIRONMENTS', environments)
    },
    deleteEnvironment: async (context, environmentName) => {
      let appId = context.getters.getActiveApplication.app.id
      await appService.deleteEnvironment(appId, environmentName)
      context.commit('DELETE_ENVIRONMENT', environmentName)
    }
  }
})
