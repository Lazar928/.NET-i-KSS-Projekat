import { createRouter, createWebHistory } from 'vue-router'
import Login from '../views/Login.vue'
import Home from '../views/Home.vue'
import Register from '../views/Register.vue'

const routes = [
  {
    path: '/login',
    component: Login
  },
  {
    path: '/home',
    component: Home
  },
  {
    path: '/register',
    component: Register
  },
  {
    path: '/',
    redirect: '/login'
  },
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

// ROUTE GUARD
router.beforeEach((to, from, next) => {
  const token = localStorage.getItem('token') 

  
  if (token && (to.path === '/login' || to.path === '/register')) {
    next('/home')
    return
  }

  
  if (!token && to.path !== '/login' && to.path !== '/register') {
    next('/login')
    return
  }

  
  next()
})

export default router
