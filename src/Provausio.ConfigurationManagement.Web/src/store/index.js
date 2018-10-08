import Vue from 'vue'
import Vuex from 'vuex'
import AppService from '../services/ApplicationService'
import xid from 'xid-js'

Vue.use(Vuex)

const appService = new AppService()
const defaultApp = { activeTab: 0, app: {} }

function getNewTab() {
  return {
      id: xid.next(),
      name: '+ Add',
      configuration: {
        content: '// add configuration here',
        format: 'javascript',
        metadata: {}
      },
      metadata: { isNew: true }
  }
}

export default new Vuex.Store({
  state: {
    applications: {},
    activeApplication: defaultApp,
    activeModals: {},
    toastMessage: {
      message: '',
      timestamp: 0
    }
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
    DELETE_APPLICATION: (state, id) => {
      delete state.applications[id]
    },
    ADD_ENVIRONMENT: (state, payload) => {
      state.activeApplication.app.environments.push(payload)
    },
    SET_ENVIRONMENTS: (state, environments) => {  
      Vue.set(state.activeApplication.app, 'environments', environments)     
    },
    SET_ENVIRONMENT_ID: (state, id) => {
      let currentEnvironment = 
        state.activeApplication.app.environments[state.activeApplication.activeTab];

        Vue.set(currentEnvironment, 'id', id)
    },
    DELETE_ENVIRONMENT: (state) => {
      state.activeApplication.app.environments
        .splice(state.activeApplication.activeTab, 1)
      state.activeApplication.activeTab = 0
    },
    SET_ENVIRONMENT_CONFIG: (state, payload) => {
      let currentEnvironment = 
        state.activeApplication.app.environments[state.activeApplication.activeTab];
        Vue.set(currentEnvironment, 'configuration', payload)
    },
    SET_ENVIRONMENT_CONFIGLANG: (state, payload) => {
      let currentEnvironmentConfig = 
        state.activeApplication.app.environments[state.activeApplication.activeTab].configuration;
      Vue.set(currentEnvironmentConfig, 'format', payload)
    },
    ACTIVATE_TAB: (state, tab) => {
      state.activeApplication.activeTab = tab
    },
    SHOW_MODAL: (state, id) => {
      Vue.set(state.activeModals, id, true)            
    },
    HIDE_MODAL: (state, id) => {
      Vue.set(state.activeModals, id, false)
    },
    SHOW_TOAST: (state, message) => {
      Vue.set(state, 'toastMessage', {
        message: message,
        timestamp: Date.now()
      })
    }
  },

  getters: {
    showModal: state => id => state.activeModals[id],    
    getApplications: state => {
      // because they're stored as objects instead of arrays
      return Object.keys(state.applications)
        .map(k => state.applications[k] || {})
    },
    getEnvironments: (state) => {     
      return state.activeApplication.app.environments
    },
    getActiveApplication: state =>  state.activeApplication,
    getEnvironmentIsNew: state => {
      return state.activeApplication.app
        && state.activeApplication.app.environments
        && state.activeApplication.app.environments[state.activeApplication.activeTab]
        && state.activeApplication.app.environments[state.activeApplication.activeTab].metadata
        && state.activeApplication.app.environments[state.activeApplication.activeTab].metadata.isNew
    },
    getActiveEnvironment: state => {
      return state.activeApplication.app
        && state.activeApplication.app.environments
        && state.activeApplication.app.environments[state.activeApplication.activeTab]
    }
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
    deleteApplication: async (context, id) => {
      await appService.deleteApplication(id)
      context.commit('DELETE_APPLICATION', id)
    },
    addingEnvironment: (context) => {
      context.commit('ADD_ENVIRONMENT', getNewTab())
    },
    createEnvironment: async (context, payload) => {
      let appId = context.getters.getActiveApplication.app.id
      let environmentId = await appService.createEnvironment(appId, payload)
      if(!environmentId) return
      context.commit('SET_ENVIRONMENT_ID', environmentId)
    },
    getEnvironments: async (context) => {
      let appId = context.getters.getActiveApplication.app.id
      let environments = await appService.getEnvironments(appId)      
      environments.push(getNewTab())
      context.commit('SET_ENVIRONMENTS', environments)
    },
    deleteEnvironment: async (context, id) => {
      let appId = context.getters.getActiveApplication.app.id
      await appService.deleteEnvironment(appId, id)
      context.commit('DELETE_ENVIRONMENT')
    }
  }
})
