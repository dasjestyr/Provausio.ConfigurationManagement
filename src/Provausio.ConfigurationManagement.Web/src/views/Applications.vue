<template>
    <div>
        <h1>Applications</h1>
        <v-flex >
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
    </div>
</template>

<script>
    export default {
        name: 'applications',
        data: () => ({
            applications: [{
                appId: 1,
                name: 'Application 1',
                description: 'Some application called 1'
            }, {
                appId: 2,
                name: 'Application 2',
                description: 'Some application called 2'
            },{
                appId: 3,
                name: 'Application 3',
                description: 'Some application called 3'
            }]
        }),
        methods: {
            goToApp(id) {
                console.log(id)
            }
        }
    }
</script>