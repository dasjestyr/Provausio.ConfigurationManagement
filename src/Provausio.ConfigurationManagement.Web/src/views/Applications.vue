<template>
    <v-flex xs12>
        <h1>Applications / Components</h1>
        
        <v-layout row>
            <v-icon color="secondary" style="font-size: 30px" @click="confirmNewApp">add_to_queue</v-icon>                   
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
        
        <new-app-diag 
            :id="newAppModalId"
            :model="newApplication"
            :confirmCallback="createNew"></new-app-diag>
    </v-flex>      
</template>

<script>
import NewApp from '@/components/NewApplication'

export default {
    name: 'applications',
    components: {
        'new-app-diag': NewApp
    },
    data: () => ({
        newAppModalId: 'new-app',
        newApplication: { name: '', description: ''},
        searchTerm: ""
    }),
    methods: {        
        confirmNewApp() {
            this.$store.commit('SHOW_MODAL', this.newAppModalId)
        },
        async createNew() {
            await this.$store.commit('HIDE_MODAL', this.newAppModalId)
            await this.$store.dispatch('addApplication', this.newApplication)
            this.$store.commit('SHOW_TOAST', `Created ${this.newApplication.name}`)
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