<template>
    <v-flex xs12>
        <h1>Applications</h1>
        <v-flex my-2>
            <v-icon color="secondary" style="font-size: 30px" @click="createNew">add_to_queue</v-icon>        
        </v-flex>        
        <v-list two-line>
            <template v-for="app in applications">
                <v-list-tile :key="app.appId" @click="goToApp(app.appId)">
                    <v-list-tile-content>
                        <v-list-tile-title>{{ app.name }}</v-list-tile-title>
                        <v-list-tile-sub-title>{{ app.description }}</v-list-tile-sub-title>
                    </v-list-tile-content>
                </v-list-tile>
            </template>
        </v-list>
    </v-flex>      
</template>

<script>
    export default {
        name: 'applications',
        data: () => ({
            applications: [{
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
        }),
        methods: {
            createNew() {
                // TODO open dialog
                console.log('create new')
            },
            goToApp(id) {
                console.log(id)
            }
        }
    }
</script>