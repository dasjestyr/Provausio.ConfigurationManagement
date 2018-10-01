<template>
    <v-card>
        <v-layout pa-4 column>
            <v-flex>
                <v-text-field 
                    color="secondary" 
                    hint="Environment name" 
                    persistent-hint
                    @change="dirty(env.id)"
                    v-model="env.name"></v-text-field>
            </v-flex>
            <v-flex>
                <v-text-field 
                    color="secondary" 
                    hint="Environment description" 
                    persistent-hint
                    v-model="env.description"></v-text-field>
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
import xid from 'xid-js'

export default {
    props: ['env'],
    data: () => ({
        hasCreated: false
    }),
    methods: {
        async dirty(id) {
            if(!this.env.metadata.isNew) return;

            if(this.hasCreated && this.env.name === ''){
                console.log(`deleted ${id}`)
            } else if(!this.hasCreated) {                
                await this.$store.dispatch('addingEnvironment')
                this.hasCreated = true
            }
        },
        async deleteEnvironment(name) {
            this.$store.dispatch('deleteEnvironment', name)
        },
        async saveEnvironment(envName) {
            if(this.env.metadata.isNew) {
                this.$store.dispatch('addEnvironment', this.env)
            } else {
                
            }
        }
    },
    watch: {
        environmentName() {
            if(this.hasCreated && this.env.name === '')
                this.$store.dispatch('deleteEnvironment', this.env.id)
        }
    },
    computed: {
        environmentName() {
            return this.env.name
        }
    },
    components: {
        'code-editor': CodeEditor
    }    
}

</script>