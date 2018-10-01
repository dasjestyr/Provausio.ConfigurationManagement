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

export default {
    components: {
        'editor': EditEnvironment
    },
    computed: {
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
