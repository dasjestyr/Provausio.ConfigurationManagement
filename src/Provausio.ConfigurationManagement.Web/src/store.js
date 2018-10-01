import Vue from 'vue'
import Vuex from 'vuex'
import AppService from './services/ApplicationService'

Vue.use(Vuex)

const appService = new AppService()

export default new Vuex.Store({
  state: {
    applications: {},
    activeApplication: {}
  },  
  mutations: {    
    SET_APPLICATIONS: (state, payload) => {
      state.applications = payload
    },
    SET_ACTIVE_APPLICATION: (state, payload) => {
      state.activeApplication = payload
    },
    ADD_APPLICATION: (state, payload) => {
      state.applications.push(payload)
    },
    ADD_ENVIRONMENT: (state, appId, payload) => {
      state.applications[appId].environments.push(payload)
    },
    SET_ENVIRONMENTS: (state, args) => {   
      state.activeApplication.environments = args.environments
    },
    DELETE_ENVIRONMENT: (state, appId, environmentName) => {
      state.applications[appId].environments =
        state.applications[appId].environments.filter(env => {
          if(env.name !== environmentName) return env;
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
      return Object.keys(state.activeApplication.environments)
        .map(k => {
          if(!k || !state.activeApplication.environments[k]) return
          return state.activeApplication.environments[k]
        })
    },
    getActiveApplication: state =>  state.activeApplication
  },
  actions: {
    setApplications: (context, applications) => {      
      context.commit('SET_APPLICATIONS', applications)
    },
    setActiveApplication: (context, application) => {
      context.commit('SET_ACTIVE_APPLICATION', application)
    },
    setApplicationsFromServer: async (context) => {
      const applications = await appService.getApplications()    
      if(applications) context.commit('SET_APPLICATIONS', applications)
    },
    addApplication: async (context, payload) => {
      payload.id = await appService.createApplication(payload)
      context.commit('ADD_APPLICATION', payload)
    },
    addEnvironment: (context, appId, payload) => {
      context.commit('ADD_ENVIRONMENT', appId, payload)
    },
    setEnvironments: async (context, args) => {
      const environments = await appService.getEnvironments(args.appId)
      
      if(args.staticTabs)
        environments.push(...args.staticTabs)

      context.commit('SET_ENVIRONMENTS', {
        appId: args.appId, 
        environments: environments
      })
    },
    deleteEnvironment: (context, appId, environmentName) => {
      context.commit('DELETE_ENVIRONMENT', appId, environmentName)
    }
  }
})
