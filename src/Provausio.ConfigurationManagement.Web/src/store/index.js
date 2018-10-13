import Vue from 'vue'
import Vuex from 'vuex'
import AppService from '../services/ApplicationService'

import ui from './ui'
import application from './application'

Vue.use(Vuex)

const appService = new AppService()

export default new Vuex.Store({

  modules: {
    ui,
    application
  },

  state: {
    applications: {},
  },  

  mutations: {    
    SET_APPLICATIONS: (state, payload) => {
      state.applications = payload
    },
    SET_ACTIVE_APPLICATION: (state, payload) => {      
      state.activeApplication = payload
    },    
    ADD_APPLICATION: (state, payload) => {
      state.applications[payload.id] = payload
    },
    REMOVE_APPLICATION: (state, id) => {
      delete state.applications[id]
    }
  },

  getters: {    
    getApplications: state => {
      return Object.keys(state.applications)
        .map(k => state.applications[k] || {})
    }
  },

  actions: {
    getApplications: async (context) => {
      let applications = await appService.getApplications()        
      if(applications) context.commit('SET_APPLICATIONS', applications)
    },    
    createApplication: async (context, payload) => {
      payload.id = await appService.createApplication(payload)
      context.commit('ADD_APPLICATION', payload)
      context.commit('ui/SHOW_TOAST', `Created ${payload.name}!`)
    }
  }
})
