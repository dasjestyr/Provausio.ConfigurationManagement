<template>
  <v-app id="inspire" dark>
    <v-navigation-drawer
      v-model="drawer"
      clipped
      fixed
      app
    >
      <v-list dense>
        <template v-for="area in areas">
          <v-list-tile :key="area.path" @click="navigate(area.path)">
            <v-list-tile-action>
              <v-icon color="secondary">{{area.icon}}</v-icon>
            </v-list-tile-action>
            <v-list-tile-content>
              <v-list-tile-title>{{ area.name }}</v-list-tile-title>
            </v-list-tile-content>
          </v-list-tile>
        </template>        
      </v-list>      
    </v-navigation-drawer>

    <v-toolbar app fixed clipped-left color="primary">
      <!-- <v-toolbar-side-icon @click.stop="drawer = !drawer"></v-toolbar-side-icon> -->
      <v-toolbar-title>Configuration Management System</v-toolbar-title>
    </v-toolbar>

    <v-content>
      <v-container fluid fill-height>
        <v-layout>
          <router-view></router-view>
        </v-layout>
      </v-container>
    </v-content>

    <v-footer app fixed>
      <span>&copy; 2017 Provausio</span>
    </v-footer>
  </v-app>
</template>

<script>
  export default {
    data: () => ({
      drawer: true,
      areas: [{
        name: 'Applications',
        path: '/applications',
        icon: 'dashboard'
      },{
        name: 'Users',
        path: '/users',
        icon: 'account_circle'
      },{
        name: 'Settings',
        path: '/settings',
        icon: 'settings'
      }]
    }),
    methods: {
      navigate(path) {
        this.$router.push(path)
      }
    },
    props: {
      source: String
    }
  }
</script>