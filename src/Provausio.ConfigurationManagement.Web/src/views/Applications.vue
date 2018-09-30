<template>
    <v-flex xs12>
        <h1>Applications</h1>
        
        <v-layout row>
            <v-icon color="secondary" style="font-size: 30px" @click="createNew">add_to_queue</v-icon>                   
            <v-text-field style="margin-left: 15px" color="accent" label="search" v-model="searchTerm"></v-text-field>    
        </v-layout>
        
        <v-slide-y-transition class="py-0" group tag="v-list">        
            <template v-for="app in filteredApplications">
                <v-flex :key="app.appId" py-2>
                    <v-list-tile  @click="goToApp(app.appId)">
                        <v-list-tile-content>
                            <v-list-tile-title>{{ app.name }}</v-list-tile-title>
                            <v-list-tile-sub-title>{{ app.description }}</v-list-tile-sub-title>
                        </v-list-tile-content>
                    </v-list-tile>
                </v-flex>
            </template>         
        </v-slide-y-transition>
    </v-flex>      
</template>

<script>
    export default {
        name: 'applications',
        data: () => ({
            searchTerm: "",
            applications: []
        }),
        methods: {
            createNew() {
                // TODO open dialog
                console.log('create new')
            },
            goToApp(id) {
                console.log(id)
            }
        },
        mounted(){
            this.applications = [{
                appId: 1,
                name: 'DAS.Services.Notifications',
                description: 'Handles email and SMS messaging'
            }, {
                appId: 2,
                name: 'DAS.Services.Surveys.Command',
                description: 'Command host for surveys'
            },{
                appId: 3,
                name: 'DAS.Services.Surveys.Query',
                description: 'Query host for surveys'
            },{
                appId: 4,
                name: 'DAS.Services.Reviews.Mgr.Command',
                description: 'Command host for review management (review response)'
            },{
                appId: 5,
                name: 'DAS.Data.ETL.Reviews',
                description: 'Review ingestion extraction orchestrator.'
            }]
        },
        computed: {
            filteredApplications() {
                let vm = this
                return vm.applications.filter(item => {
                        return item.name.toLowerCase().indexOf(vm.searchTerm.toLowerCase()) !== -1
                    })
            }
        }
    }
</script>