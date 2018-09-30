<template>
    <v-flex>     
        <v-flex my-2>
            <v-text-field color="secondary" hint="Application Name" persistent-hint v-model="application.name"></v-text-field>
            <v-textarea color="secondary" hint="Description of the application" persistent-hint v-model="application.description"></v-textarea>  
        </v-flex>      
        
        <h2>Environments</h2>

        <v-flex my-4>            
            <v-tabs color="secondary" slider-color="white" v-model="activeTab">
                <v-tab 
                    v-for="env in environments" 
                    :key="env.name">
                        {{env.name}}
                </v-tab>
                <v-tab-item
                    v-for="env in environments"
                    :key="env.name">
                    <v-card>
                        <v-flex pa-4>
                            <h3>{{ env.description }}</h3>
                            <code-editor :editor-id="env.name + '-editor'" :lang="env.format" :content="env.configuration"></code-editor>
                            <v-flex mt-2><v-btn round color="success" @click="saveEnvironment(env.name)">Save {{ env.name }} configuration</v-btn></v-flex>
                        </v-flex>
                    </v-card>
                </v-tab-item>
            </v-tabs>
        </v-flex>
    </v-flex>    
    
</template>

<script>
import AppService from '../services/ApplicationService.js'
import CodeEditor from '../components/CodeEditor'
const appService = new AppService()
export default {
    data: () => ({
        application: {},
        environments: [],
        activeTab: 0
    }),
    methods: {
        async saveEnvironment(env) {
            const environment = { name: env }
            await appService.saveEnvironment(environment)
        }
    },
    async mounted() {
        this.application = await appService.getApplication(this.$route.params.id)   
        this.environments = await appService.getEnvironments(this.application.id)   
    },
    components: {
        'code-editor': CodeEditor
    }
}
</script>
