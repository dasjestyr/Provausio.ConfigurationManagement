import Vue from 'vue'
import Router from 'vue-router'

Vue.use(Router)

export default new Router({
  mode: 'history',
  base: process.env.BASE_URL,
  routes: [
    {
      path: '/',
      name: 'applications',
      component: () => import('./views/Applications.vue')
    },
    {
      path: '/applications',
      name: 'applications',
      component: () => import('./views/Applications.vue')
    },
    {
      path: '/applications/:id',
      name: 'application-detail',
      component: () => import('./views/ApplicationDetail.vue')
    }
  ]
})
