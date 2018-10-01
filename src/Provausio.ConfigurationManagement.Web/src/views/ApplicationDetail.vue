<template>
    <v-flex>     
        <v-flex my-2>
            <v-text-field 
                color="secondary" 
                hint="Application Name" 
                persistent-hint 
                v-model="activeApplication.app.name"
                v-if="activeApplication.app"></v-text-field>
            <v-text-field 
                color="secondary" 
                hint="Description of the application" 
                persistent-hint 
                v-model="activeApplication.app.description"
                v-if="activeApplication.app"></v-text-field>  
        </v-flex>      
        
        <h2>Environments</h2>

        <v-flex my-4>            
            <v-tabs color="secondary" slider-color="white" v-model="activeApplication.activeTab">
                <v-tab 
                    v-if="activeApplication.app"
                    v-for="env in environments" 
                    :key="env.name">
                        {{ env.name }}
                </v-tab>
                <v-tab-item
                    v-for="env in environments"
                    :key="env.name">
                        <environment-editor :env="env"></environment-editor>                    
                </v-tab-item>
            </v-tabs>
        </v-flex>
    </v-flex>       
</template>

<script>

import EditEnvironment from '../components/EditEnvironment'
export default {
    data: () => ({
        loading: true,
        loadingMessage: 'Hold on to your butts'
    }),
    computed: {
        activeApplication() {
            return this.$store.getters.getActiveApplication
        },
        environments() {                   
            return this.$store.getters.getEnvironments                        
        }
    },
    async created() {
                
        const newEnvironmentTab = [{
            id: 'newtab',
            name: "+ New",
            format: 'javascript',
            configuration: '// add configuration here',
            metadata: { isNew: true, canEditName: true }
        }]

        await this.$store.dispatch('getApplication', this.$route.params.id)
        await this.$store.dispatch('getEnvironments', newEnvironmentTab)
        
        this.activeTab = 0
        this.loading = false
    },
    async beforeDestroy() {
        // this prevents the data flip-over blip when switching between apps
        await this.$store.dispatch('clearApplication')
    },
    components: {
        'environment-editor': EditEnvironment
    }
}
</script>
