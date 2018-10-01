<template>
    <v-flex>     
        <v-flex my-2>
            <v-text-field color="secondary" hint="Application Name" persistent-hint v-model="application.name"></v-text-field>
            <v-text-field color="secondary" hint="Description of the application" persistent-hint v-model="application.description"></v-text-field>  
        </v-flex>      
        
        <h2>Environments</h2>

        <v-flex my-4>            
            <v-tabs color="secondary" slider-color="white" v-model="activeTab">
                <v-tab 
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
import AppService from '../services/ApplicationService.js'
import EditEnvironment from '../components/EditEnvironment'
const appService = new AppService()
export default {
    data: () => ({
        application: {},
        newEnvironment: {},
        activeTab: 0
    }),
    methods: {
        async saveEnvironment(env) {
            const environment = { name: env }
            await appService.saveEnvironment(environment)
        }
    },
    computed: {
        environments() {          
            return this.$store.getters.getEnvironments                        
        }
    },
    async mounted() {
        this.application = await appService.getApplication(this.$route.params.id)   
        const newEnvironmentTab = [{
            name: "+ New",
            format: 'javascript',
            configuration: '// add configuration here',
            metadata: { isNew: true }
        }]
        await this.$store.dispatch('setActiveApplication', this.application)
        await this.$store.dispatch('setEnvironments', {
                    appId: this.$route.params.id, 
                    staticTabs: newEnvironmentTab})
        
        this.activeTab = 0
    },
    components: {
        'environment-editor': EditEnvironment
    }
}
</script>
