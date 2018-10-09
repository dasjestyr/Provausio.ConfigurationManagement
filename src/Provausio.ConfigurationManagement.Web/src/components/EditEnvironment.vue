<template>
    <v-card>
        <v-layout pa-4 column>
            <v-flex>
                <v-text-field 
                    color="secondary" 
                    hint="Environment name" 
                    persistent-hint
                    @keypress="dirty(env.id)"
                    v-model="name"></v-text-field>
            </v-flex>
            <v-flex>
                <v-text-field 
                    color="secondary" 
                    hint="Environment description" 
                    persistent-hint
                    v-model="description"></v-text-field>
            </v-flex>                
        </v-layout>
        <v-flex pa-4>
            <code-editor :content="content"></code-editor>
            <v-flex mt-2>
                <v-btn round v-if="!environment.metadata.isNew" color="error" @click="deleteEnvironment(env.id)">Delete {{ environment.name }}</v-btn>
                <v-btn round color="success" @click="saveEnvironment(env.name)">Save {{ environment.name }}</v-btn>
            </v-flex>
        </v-flex>        
    </v-card>
</template>

<script>
import CodeEditor2 from './CodeEditor2'
import {mapState, mapMutations, mapActions} from 'vuex'
import svc from '@/services/ApplicationService'
const appService = new svc()
export default {
    components: {
        'code-editor': CodeEditor2
    },
    props: ['env'],
    data: () => ({
        hasCreated: false,        
    }),
    methods: {
        ...mapActions('application', {
            save: 'saveEnvironment',
            delete: 'deleteEnvironment'
        }),
        async dirty(id) {
            if(!this.env.metadata.isNew) return;

            if(this.hasCreated && this.env.name === ''){
                console.log(`deleted ${id}`)
            } else if(!this.hasCreated) {                
                await this.$store.dispatch('application/addingEnvironment')
                this.hasCreated = true
            }
        },
        async deleteEnvironment(id) {
            await this.delete()         
            this.$store.commit('ui/SHOW_TOAST', `Deleted ${name} environment.`)
        },
        async saveEnvironment(name) {            
            this.save()
            this.$store.commit('ui/SHOW_TOAST', `Saved ${name} environment.`)
        }
    },
    computed: {
        ...mapState('application', {
            application: 'current',
            environment: 'activeEnvironment'
        }),
        content: {
            get() {
                return this.environment.configuration.content
            },
            set(v) {
                this.$store.commit('application/SET_ENVIRONMENT_CONFIG_CONTENT', v)
            }
        },
        name: {
            get() {
                return this.environment.name
            },
            set(v) {
                this.$store.commit('application/SET_ENVIRONMENT_NAME', v)
            }
        },
        description: {
            get() {
                return this.environment.description
            },
            set(v) {
                this.$store.commit('application/SET_ENVIRONMENT_DESCRIPTION', v)
            }
        }
    }   
}

</script>