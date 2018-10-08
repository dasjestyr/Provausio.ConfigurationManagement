import AppService from '@/services/ApplicationService'
import Vue from 'vue'

const appService = new AppService()

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

export default {    
    namespaced: true,
    state: {
        current: {},
        activeTab: 0,
        environments: [],
        activeEnvironment: {}
    },

    getters: {
        getEnvironmentIsNew(state) {
            return state.activeEnvironment 
                && state.activeEnvironment.metadata
                && state.activeEnvironment.metadata.isNew
        }
    },

    mutations: {
        SET_APPLICATION(state, payload) {
            Vue.set(state, 'current', payload)
        },
        DELETE_APPLICATION(state, payload) {
            // TODO: notify parent?
        },
        CLEAR_ACTIVE_APPLICATION(state) {
            state.current = null
        },
        SET_APPLICATION_NAME(state, payload) {
            state.current.name = payload
        },
        SET_ENVIRONMENTS(state, payload) {
            state.environments = payload
            state.activeTab = 0
        },
        ADD_ENVIRONMENT(state, payload) {
            state.environments.push(payload)            
        },
        DELETE_ENVIRONMENT(state, environmentId) {
            let remainingEnvironments = state.environments.filter(env => {
                if(env.id !== environmentId) return env
            })
            state.environments = remainingEnvironments
        },
        SET_ACTIVE_ENVIRONMENT(state, tabIndex) {
            state.activeTab = tabIndex
            state.activeEnvironment = state.environments[tabIndex]
        },    
        SET_ENVIRONMENT_CONFIG(state, payload) {
            let currentEnvironment = 
              state.activeApplication.app.environments[state.activeApplication.activeTab];
              Vue.set(currentEnvironment, 'configuration', payload)
          },
        SET_ENVIRONMENT_CONFIGLANG(state, payload) {
            let currentEnvironmentConfig = 
                state.activeApplication.app.environments[state.activeApplication.activeTab].configuration;
            Vue.set(currentEnvironmentConfig, 'format', payload)
        },    
    },

    actions: {
        async getApplication(context, applicationId) {
            let application = await appService.getApplication(applicationId)
            await context.commit('SET_APPLICATION', application)
            await context.commit('SET_ACTIVE_ENVIRONMENT', 0)
        },
        async deleteApplication(context, applicationId) {
            await appService.deleteApplication(applicationId)
            context.commit('DELETE_APPLICATION')
        },
        async getEnvironments(context) {
            let environments = await appService.getEnvironments(context.state.current.id)
            context.commit('SET_ENVIRONMENTS', environments)
        },
        async createEnvironment(context, payload) {
            payload.id = await appService.createEnvironment(context.state.current.id, payload)
            if(!environmentId) return
            context.commit('ADD_ENVIRONMENT', payload)
        },
        async deleteEnvironment(context, environmentId) {
            await appService.deleteEnvironment(context.state.current.id, environmentId)
            context.commit('DELETE_ENVIRONMENT', environmentId)
        },
        addingEnvironment: (context) => {
            context.commit('ADD_ENVIRONMENT', getNewTab())
        }
    }
}