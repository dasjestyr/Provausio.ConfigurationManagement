<template>
    <v-flex xs12>
        <h1>Applications / Components</h1>
        
        <v-layout row>
            <v-icon color="secondary" style="font-size: 30px" @click="newAppDialog = true">add_to_queue</v-icon>                   
            <v-text-field style="margin-left: 15px" color="accent" label="search" v-model="searchTerm"></v-text-field>    
        </v-layout>
        
        <v-slide-y-transition class="py-0" group tag="v-list">        
            <template v-for="app in filteredApplications">
                <v-flex :key="app.id" py-2>
                    <v-list-tile  @click="goToApp(app.id)">
                        <v-list-tile-content>
                            <v-list-tile-title>{{ app.name }}</v-list-tile-title>
                            <v-list-tile-sub-title>{{ app.description }}</v-list-tile-sub-title>
                        </v-list-tile-content>
                    </v-list-tile>
                </v-flex>
            </template>         
        </v-slide-y-transition>

        <!-- TODO: move to a separate component -->
        <v-layout row justify-center>
            <v-dialog v-model="newAppDialog" persistent max-width="500px">
                <v-card>
                    <v-card-title>
                        <span class="headline">Create a new application</span>
                    </v-card-title>
                    <v-card-text>
                        <v-container grid-list-md>
                            <v-layout wrap>
                                <v-flex xs12>
                                    <v-text-field color="secondary"
                                        v-model="newApplication.name"
                                        required
                                        hint="Name of the application"
                                        label="Name"></v-text-field>
                                </v-flex>
                                <v-flex xs12>
                                    <v-textarea color="secondary"
                                        v-model="newApplication.description"
                                        required
                                        hint="What does the application do?"
                                        label="Description"></v-textarea>
                                </v-flex>
                            </v-layout>
                        </v-container>
                    </v-card-text>
                    <v-card-actions style="margin-right: 5px">
                        <v-spacer></v-spacer>
                        <v-btn flat @click.native="newAppDialog = false">Cancel</v-btn>
                        <v-btn color="secondary" @click.native="createNew">Create</v-btn>
                    </v-card-actions>
                </v-card>
            </v-dialog>        
        </v-layout>
    </v-flex>      
</template>

<script>
export default {
    name: 'applications',
    data: () => ({
        newAppDialog: false,
        newApplication: { name: '', description: ''},
        searchTerm: ""
    }),
    methods: {        
        async createNew() {
            this.newAppDialog = false            
            await this.$store.dispatch('addApplication', this.newApplication)
            this.$router.push(`applications/${this.newApplication.id}`)
        },
        goToApp(id) {
            this.$router.push(`applications/${id}`)
        }
    },
    async mounted(){        
        this.$store.dispatch('getApplicationsFromServer')      
    },
    computed: {
        filteredApplications() {       
            let vm = this     
            return vm.$store.getters.getApplications.filter(item => {
                    if(!vm.searchTerm) return item;
                    return item.name.toLowerCase().indexOf(vm.searchTerm.toLowerCase()) !== -1
                })
        }
    }
}
</script>