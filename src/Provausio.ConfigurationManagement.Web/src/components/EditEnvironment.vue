<template>
    <v-card>
        <v-layout pa-4 column>
            <v-flex>
                <v-text-field color="secondary" hint="Environment name" persistent-hint="" v-model="env.name"></v-text-field>
            </v-flex>
            <v-flex>
                <v-text-field color="secondary" hint="Environment description" persistent-hint="" v-model="env.description"></v-text-field>
            </v-flex>                
        </v-layout>
        <v-flex pa-4>
            <code-editor :editor-id="env.name + '-editor'" :lang="env.format" :content="env.configuration"></code-editor>
            <v-flex mt-2>
                <v-btn round v-if="!env.metadata.isNew" color="error" @click="deleteEnvironment(env.name)">Delete {{ env.name }}</v-btn>
                <v-btn round color="success" @click="saveEnvironment(env.name)">Save {{ env.name }}</v-btn>
            </v-flex>
        </v-flex>        
    </v-card>
</template>

<script>
import CodeEditor from './CodeEditor'
import AppService from '../services/ApplicationService'
const appService = new AppService()
export default {
    props: ['env'],
    data: () => ({
        
    }),
    methods: {
        async deleteEnvironment(env) {
            // will need to update redux to have the tab remove itself after deleting on the server
            await appService.deleteEnvironment(env)
        },
        async saveEnvironment(envName) {
            if(this.env.metadata.isNew) {
                await appService.createEnvironment(this.env)
                // TODO: tell redux to add the tab
            } else {
                await appService.saveEnvironment(this.env)
            }
        }
    },
    components: {
        'code-editor': CodeEditor
    }
}
</script>