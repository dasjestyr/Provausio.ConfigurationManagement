import Vue from 'vue'
import Vuetify from 'vuetify'
import 'vuetify/dist/vuetify.min.css'

// temp
let defaultTheme = {
  theme: {
    primary: '#ee44aa',
    secondary: '#424242',
    accent: '#82B1FF',
    error: '#FF5252',
    info: '#2196F3',
    success: '#4CAF50',
    warning: '#FFC107'
  },
  customProperties: true,
  iconfont: 'fa',
}

let experimentalTheme = {
  theme: {
    primary: '#1D4370',
    secondary: '#326EB6',
    accent: '#43701D',
    error: '#b71c1c'
  },
  customProperties: true,
  iconfont: 'fa',
}

Vue.use(Vuetify, experimentalTheme)

