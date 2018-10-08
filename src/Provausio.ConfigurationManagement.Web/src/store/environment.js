import AppService from '@/services/ApplicationService'

const appService = new AppService()

export default {
    state : {
        name: '',
        description: '',
        configuration: {
            format: '',
            content: '',
            metdata: {}
        },
        metadata: {}
    },

    getters: {
        
    },

    mutations: {
        setName(state, payload){
            state.name = payload
        },
        setDescription(state, payload){
            state.description = payload
        },
        setConfigFormat(state, payload){
            Vue.set(state.configuration, 'format', payload)
        },
        setConfigContent(state, payload){
            Vue.set(state.configuration, 'content', payload)
        }        
    },

    actions: {
        
    }
}