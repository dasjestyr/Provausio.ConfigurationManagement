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
export default {
    props: ['content', 'selectedLanguage'],
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
        language: {
            get() {
                return this.$store.getters.getActiveEnvironment.configuration.format
            },
            set(v) {
                this.$store.commit('SET_ENVIRONMENT_CONFIGLANG', v)
            }
        }
    }
}
</script>