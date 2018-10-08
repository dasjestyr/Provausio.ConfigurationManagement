import Vue from 'vue'

export default {
    namespaced: true,
    state: {         
        activeModals: {},
        toastMessage: {
            message: '',
            timestamp: 0
        }
    },

    getters: {
        showModal: state => id => !!state.activeModals[id]
    },

    mutations: {
        SHOW_MODAL(state, id) {
            Vue.set(state.activeModals, id, true)
        },
        HIDE_MODAL(state, id) {
            Vue.set(state.activeModals, id, false)
        },
        SHOW_TOAST(state, message) {
            Vue.set(state, 'toastMessage', {
                message: message,
                timestamp: Date.now() // ensures message will display if same as last message
            })
        }
    },

    actions: {

    }
}