import AppService from '@/services/ApplicationService'
import xid from 'xid-js'
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
        activeTab(state) {
            return state.activeTab
        },
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
            state.activeEnvironment = state.environments[0]
            state.activeTab = 0
        },
        ADD_ENVIRONMENT(state, payload) {
            state.environments.push(payload)            
        },
        COMMIT_NEW_ENVIRONMENT(state, env) {
            console.log(env)
            Vue.set(state.activeEnvironment, 'activeEnvironment', env)    
            Vue.set(state.activeEnvironment, 'name', env.name)        
        },
        async DELETE_ENVIRONMENT(state, environmentId) {
            let remainingEnvironments = state.environments.filter(env => {
                if(env.id !== environmentId) return env
            })
            state.environments = remainingEnvironments            
        },
        SET_ACTIVE_ENVIRONMENT(state, tabIndex) {
            state.activeTab = tabIndex
            state.activeEnvironment = state.environments[tabIndex]
        },    
        SET_ENVIRONMENT_NAME(state, name){
            state.activeEnvironment.name = name
        },
        SET_ENVIRONMENT_DESCRIPTION(state, description) {
            Vue.set(state.activeEnvironment, 'description', description)
        },
        SET_ENVIRONMENT_CONFIG(state, payload) {
            Vue.set(state.activeEnvironment, 'configuration', payload)
        },
        SET_ENVIRONMENT_CONFIG_CONTENT(state, payload) {
            Vue.set(state.activeEnvironment.configuration, 'content', payload)
        },
        SET_ENVIRONMENT_CONFIGLANG(state, payload) {
            Vue.set(state.activeEnvironment.configuration, 'format', payload)
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
            environments.push(getNewTab())
            context.commit('SET_ENVIRONMENTS', environments)
        },
        async saveEnvironment(context) {
            const wasNew = context.state.activeEnvironment.metadata.isNew
            let env = await appService.saveEnvironment(context.state.current.id, context.state.activeEnvironment)
            if(wasNew) {
                context.commit('COMMIT_NEW_ENVIRONMENT', env)
            }
        },
        async deleteEnvironment(context) {
            if(!context.state.activeEnvironment.id){
                console.error('Cannot delete undefined!')
                return
            }
            await appService.deleteEnvironment(
                context.state.current.id, 
                context.state.activeEnvironment.id)

            await context.commit('DELETE_ENVIRONMENT', context.state.activeEnvironment.id)
            context.state.activeTab = 0
        },
        addingEnvironment: (context) => {
            let newTab = getNewTab()
            context.commit('ADD_ENVIRONMENT', newTab)
        }
    }
}