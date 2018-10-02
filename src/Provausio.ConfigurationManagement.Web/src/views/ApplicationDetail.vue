<template>
    <v-flex>     
        <v-flex my-2>
            <v-text-field 
                color="secondary" 
                hint="Application Name" 
                persistent-hint 
                v-model="activeApplication.app.name"
                ></v-text-field>
            <v-text-field 
                color="secondary" 
                hint="Description of the application" 
                persistent-hint 
                v-model="activeApplication.app.description"
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
                Are you sure you want to delete {{ activeApplication.app.name }}? 
                This will also delete all associated environments and cannot be undone!
            </dangerous>
        </v-flex>      
        
        <h2>Environments</h2>

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
    </v-flex>       
</template>

<script>

import EditEnvironment from '../components/EditEnvironment'
import ConfirmDangerous from '../components/ConfirmDangerous'

export default {
    data: () => ({
        deleteAppModalId: 'delete-app'
    }),
    components: {
        'editor': EditEnvironment,
        'dangerous': ConfirmDangerous
    },
    methods: {
        confirmAppDelete() {            
            this.$store.commit('SHOW_MODAL', this.deleteAppModalId)   
        },
        async deleteApp() {
            this.$store.commit('HIDE_MODAL', this.deleteAppModalId)
            await this.$store.dispatch('deleteApplication', this.activeApplication.app.id)
            this.$store.commit('SHOW_TOAST', `Deleted ${this.activeApplication.app.name}!`)
            this.$router.push('/applications')            
        }
    },
    computed: {
        env() {
            return this.$store.getters.getActiveEnvironment
        },
        activeApplication() {
            return this.$store.getters.getActiveApplication
        },
        environments() {                   
            return this.$store.getters.getEnvironments                        
        },
        activeTab : {
            get() {
                this.$store.getters.getActiveApplication.activeTab
            },
            set(value) {
                this.$store.commit('ACTIVATE_TAB', value)
            }
        }
    },
    async mounted() {        
        await this.$store.dispatch('getApplication', this.$route.params.id)
        await this.$store.dispatch('getEnvironments')             

        this.loading = false        
        this.activeTab = 1
    },
    async beforeDestroy() {
        // this prevents the stale data flip-over blip when switching between apps
        await this.$store.dispatch('clearApplication')
    }
}
</script>
