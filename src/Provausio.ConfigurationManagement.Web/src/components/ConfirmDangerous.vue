<template>
    <v-layout row justify-center>
        <v-dialog persistent max-width="500px" v-model="show">
            <v-card>
                <v-card-title>
                    <span class="headline">{{ header }}</span>
                </v-card-title>
                <v-card-text>
                    <slot></slot>
                </v-card-text>
                <v-card-actions>
                    <v-spacer></v-spacer>
                    <v-btn color="success" flat @click="cancel">{{ canceltext }}</v-btn>
                    <v-btn color="error" @click=confirmCallback>{{ oktext }}</v-btn>
                </v-card-actions>
            </v-card>                    
        </v-dialog>
    </v-layout>
</template>

<script>
export default {
    props: ['header', 'oktext', 'canceltext', 'id', 'confirmCallback'],
    data: () => ({
        
    }),
    methods: {
        cancel() {
            this.$store.commit('HIDE_MODAL', this.id)
        }
    },
    computed: {
        show() {            
            return this.$store.getters.showModal(this.id)
        }
    }
}
</script>