<template>
    <v-flex xs12>      
        <v-layout align-center justify-center v-if="loading">
            <v-progress-circular indeterminate :size="50" color="accent"  centered></v-progress-circular>
        </v-layout>  
        <div v-if="!loading">
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
        </div>
    </v-flex>      
</template>

<script>
import NewApp from '@/components/NewApplication'
import { mapActions } from 'vuex';

export default {
    name: 'applications',
    components: {
        'new-app-diag': NewApp
    },
    data: () => ({
        loading: true,
        newAppModalId: 'new-app',
        newApplication: { name: '', description: ''},
        searchTerm: ""
    }),
    methods: {     
        ...mapActions(['createApplication']),   
        confirmNewApp() {
            this.$store.commit('ui/SHOW_MODAL', this.newAppModalId)
        },
        async createNew() {
            await this.$store.commit('ui/HIDE_MODAL', this.newAppModalId)
            await this.createApplication(this.newApplication)
            this.$router.push(`applications/${this.newApplication.id}`)
        },
        goToApp(id) {
            this.$router.push(`applications/${id}`)
        }
    },
    async mounted(){        
        await this.$store.dispatch('getApplications')      
        this.loading = false
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