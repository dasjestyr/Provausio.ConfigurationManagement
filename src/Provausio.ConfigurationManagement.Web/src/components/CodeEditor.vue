<template>
    <v-layout column>
        <v-select color="secondary" 
            :items="languages" 
            label="format" 
            v-model="selectedLanguage" 
            style="max-width: 200px;"></v-select>   

        <div :id="editorId" style="width: 100%; height: 100%; min-height: 500px;"></div>
    </v-layout>
</template>

<script>
import { EventBus } from '@/eventbus.js'
export default {
    props: ['editorId', 'content', 'lang', 'theme'],
    data: () => ({
        editor: Object,
        beforeContent: '',
        selectedLanguage: '',
        languages: ['json', 'javascript', 'yaml', 'xml']
    }),
    methods: {
        syncFromEditor() {            
            this.$store.commit('SET_ENVIRONMENT_CONFIG', {
                content: this.editor.getValue(),
                format: this.selectedLanguage
            })
        }
    },
    watch: {
        selectedLanguage() {            
            this.editor.getSession().setMode(`ace/mode/${this.selectedLanguage}`)
            this.syncFromEditor()
        }
    },
    mounted () {
        const theme = this.theme || 'dracula'
    
        this.editor = window.ace.edit(this.editorId)
        this.editor.setValue(this.content, 1)        
        this.selectedLanguage = this.lang
        this.editor.setTheme(`ace/theme/${theme}`)

        // workaround for ace not being reactive
        // is it possible to make the textbox reactive? It seems like it would spam events
        EventBus.$on('update-editor-from', () => this.syncFromEditor())
    }    
}
</script>