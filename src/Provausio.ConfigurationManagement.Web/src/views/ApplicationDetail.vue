<template>
    <v-flex>  
        <v-layout align-center justify-center v-if="loading">
            <v-progress-circular indeterminate :size="50" color="accent"  centered></v-progress-circular>
        </v-layout>     
        <div v-if="!loading">
            <v-flex my-2>
                <v-text-field 
                    color="secondary" 
                    hint="Application Name" 
                    persistent-hint 
                    v-model="application.name"
                    ></v-text-field>
                <v-text-field 
                    color="secondary" 
                    hint="Description of the application" 
                    persistent-hint 
                    v-model="application.description"
                    ></v-text-field>  
                <v-layout mt-2 align-end justify-end>
                    <v-spacer></v-spacer>
                    <v-btn round right color="error" @click="confirmAppDelete">Delete Application</v-btn>                
                </v-layout>

                <dangerous header="Delete Application" 
                    :confirmCallback="deleteApp"
                    oktext="Confirm Delete" 
                    canceltext="Cancel" 
                    :id="deleteAppModalId">
                    Are you sure you want to delete {{ application.name }}? 
                    This will also delete all associated environments and cannot be undone!
                </dangerous>
            </v-flex>      
            
            <h2>Environments</h2>
            <v-btn flat color="success" @click="newEnvironmentDiag">Create new</v-btn>
            <v-flex my-4>            
                <v-tabs color="secondary" slider-color="white" v-model="activeTab">
                    <v-tab                     
                        v-for="env in environments" 
                        :key="env.id">
                            {{ env.name }}
                    </v-tab>
                    <v-tab-item
                        v-for="env in environments"
                        :key="env.id">
                            <editor :env="env"></editor>                    
                    </v-tab-item>
                </v-tabs>
            </v-flex>
        </div>
        <new-env-diag
            :id="newEnvModalId"
            :model="newEnvironment"
            :confirmationCallback="createNewEnv"></new-env-diag>
    </v-flex>       
    
</template>

<script>

import EditEnvironment from '../components/EditEnvironment'
import ConfirmDangerous from '../components/ConfirmDangerous'
import { mapState, mapActions, mapMutations } from 'vuex';
import NewEnvironmentDiag from '@/components/NewEnvironment'
import environment from '@/model/Environment'

export default {
    data: () => ({
        newEnvModalId: 'new-env',
        loading: true,
        deleteAppModalId: 'delete-app',
        newEnvironment: environment.create()
    }),
    components: {
        'editor': EditEnvironment,
        'new-env-diag': NewEnvironmentDiag,
        'dangerous': ConfirmDangerous
    },
    methods: {
        ...mapActions('application', {
            saveEnvironment: 'saveEnvironment'
        }),
        ...mapMutations('application', {
            clearApplication: 'CLEAR_ACTIVE_APPLICATION'
        }),
        confirmAppDelete() {            
            this.$store.commit('ui/SHOW_MODAL', this.deleteAppModalId)   
        },
        async deleteApp() {
            this.$store.commit('ui/HIDE_MODAL', this.deleteAppModalId)
            await this.$store.dispatch('application/deleteApplication', this.application.id)
            this.$store.commit('ui/SHOW_TOAST', `Deleted ${this.application.name}!`)
            this.$router.push('/applications')            
        },
        newEnvironmentDiag() {
            this.$store.commit('ui/SHOW_MODAL', this.newEnvModalId)
        },
        async createNewEnv() {
            this.newEnvironment.id = xid.next()
            await this.saveEnvironment(this.newEnvironment)
            this.newEnvironment = environment.create()
            this.$store.commit('ui/HIDE_MODAL', this.newEnvModalId)
            this.$store.commit('ui/SHOW_TOAST', `Saved ${this.newEnvironment.name}!`)
        }
    },
    computed: {
        ...mapState('application', {
            env: 'activeEnvironment',
            application: 'current',
            environments: 'environments'
        }),
        activeTab : {
            get() {
                return this.$store.getters['application/activeTab']
            },
            set(value) {
                this.$store.commit('application/SET_ACTIVE_ENVIRONMENT', value)
            }
        }
    },
    async created() {        
        await this.$store.dispatch('application/getApplication', this.$route.params.id)
        await this.$store.dispatch('application/getEnvironments')
        this.loading = false
    },
    async beforeDestroy() {
        // this prevents the stale data flip-over blip when switching between apps
        await this.clearApplication()
    }
}
</script>
