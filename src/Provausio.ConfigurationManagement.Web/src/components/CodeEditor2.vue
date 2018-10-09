<template>
    <v-layout column>
        <v-select color="secondary" 
            :items="languages" 
            v-model="language"
            label="format"             
            style="max-width: 200px;"></v-select>
        <ace-editor 
            v-model="content" 
            min-lines="50" 
            max-lines="500" 
            :mode="language"></ace-editor>        
    </v-layout>
</template>

<script>
import AceEditor from  './AceEditorVue'
import { mapState } from 'vuex';
export default {
    components: { 'ace-editor': AceEditor },
    data: () => ({
        languages: ['json', 'javascript', 'yaml', 'xml']
    }),
    methods: {
        setLanguage(val) {
            this.$store.commit('SET_ENVIRONMENT_CONFIGLANG', val)
        }
    },
    computed: {
        ...mapState('application', {
            application: 'current',
            environment: 'activeEnvironment',
        }),
        language: {
            get() {
                return this.environment.configuration.format
            },
            set(v) {
                this.$store.commit('application/SET_ENVIRONMENT_CONFIGLANG', v)
            }
        },
        content: {
            get() {
                return this.environment.configuration.content
            },
            set(v) {
                this.$store.commit('application/SET_ENVIRONMENT_CONFIG_CONTENT', v)
            }
        }
    }
}
</script>