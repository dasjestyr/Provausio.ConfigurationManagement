<template>
    <v-layout row justify-center>
        <v-dialog persistent max-width="500px" v-model="show">
            <v-card>
                <v-card-title>
                    <span class="headline">Create a new application</span>
                </v-card-title>
                <v-card-text>
                    <v-container grid-list-md>
                        <v-layout wrap>
                            <v-flex xs12>
                                <v-text-field color="secondary"
                                    v-model="model.name"
                                    required
                                    hint="Name of the application"
                                    label="Name"></v-text-field>
                            </v-flex>
                            <v-flex xs12>
                                <v-text-field color="secondary"
                                    v-model="model.description"
                                    required
                                    hint="What does the application do?"
                                    label="Description"></v-text-field>
                            </v-flex>
                        </v-layout>
                    </v-container>
                </v-card-text>
                <v-card-actions style="margin-right: 5px">
                    <v-spacer></v-spacer>
                    <v-btn flat @click=cancel>Cancel</v-btn>
                    <v-btn color="secondary" @click=confirmCallback>Create</v-btn>
                </v-card-actions>
            </v-card>
        </v-dialog>        
    </v-layout>
</template>

<script>
import { mapState, mapGetters } from 'vuex'
export default {
    props: ['id', 'confirmCallback', 'model'],
    methods: {
        cancel() {
            this.$store.commit('ui/HIDE_MODAL', this.id)
        }
    },
    computed: {
        show() {
            return this.$store.getters['ui/showModal'](this.id)
        }
    }
}
</script>