<template>
    <v-layout column>
        <v-select color="secondary" :items="languages" label="format" v-model="selectedLanguage" style="max-width: 200px;"></v-select>        
        <div :id="editorId" style="width: 100%; height: 100%; min-height: 500px;"></div>
    </v-layout>
</template>

<script>

export default {
    props: ['editorId', 'content', 'lang', 'theme'],
    data: () => ({
        editor: Object,
        beforeContent: '',
        selectedLanguage: '',
        languages: ['json', 'javascript', 'yaml', 'xml']
    }),
    watch: {
        selectedLanguage: function(lang) {
            this.editor.getSession().setMode(`ace/mode/${lang || this.lang}`)
        }
    },
    mounted () {
        const theme = this.theme || 'dracula'
    
        this.editor = window.ace.edit(this.editorId)
        this.editor.setValue(this.content, 1)
        this.selectedLanguage = this.lang
        this.editor.setTheme(`ace/theme/${theme}`)
    }    
}
</script>