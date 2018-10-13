import AppService from '@/services/ApplicationService'
import xid from 'xid-js'
import Vue from 'vue'

const appService = new AppService()

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
            context.commit('SET_ENVIRONMENTS', environments)
        },
        async saveEnvironment(context, payload) {
            if(payload) { // if payload, then it is a new environment
                let env = await appService.saveEnvironment(
                    context.state.current.id,
                    payload)
                
                await context.commit('ADD_ENVIRONMENT', env)
                
            } else { // else, save active environment
                await appService.saveEnvironment(
                    context.state.current.id, 
                    context.state.activeEnvironment)
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
        }
    }
}