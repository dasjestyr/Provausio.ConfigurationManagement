<template>
    <v-flex>     
        <v-flex my-2>
            <v-text-field color="secondary" hint="Application Name" persistent-hint v-model="activeApplication.name"></v-text-field>
            <v-text-field color="secondary" hint="Description of the application" persistent-hint v-model="activeApplication.description"></v-text-field>  
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
        activeApplication() {
            return this.$store.getters.getActiveApplication
        },
        environments() {          
            return this.$store.getters.getEnvironments                        
        }
    },
    async mounted() {
                
        const newEnvironmentTab = [{
            id: 'newtab',
            name: "+ New",
            format: 'javascript',
            configuration: '// add configuration here',
            metadata: { isNew: true }
        }]

        let application = await appService.getApplication(this.$route.params.id)   
        await this.$store.dispatch('setActiveApplication', application)
        await this.$store.dispatch('getEnvironments', newEnvironmentTab)
        
        this.activeTab = 0
    },
    components: {
        'environment-editor': EditEnvironment
    }
}
</script>
