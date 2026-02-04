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

// 🔐 ROUTE GUARD
router.beforeEach((to, from, next) => {
  const token = localStorage.getItem('token')

  // Ako je ulogovan i pokušava login ili register → prebaci na home
  if (token && (to.path === '/login' || to.path === '/register')) {
    next('/home')
    return
  }

  // Ako NIJE ulogovan i ide na zaštićenu rutu → vrati na login
  if (!token && to.path !== '/login' && to.path !== '/register') {
    next('/login')
    return
  }

  // U svim ostalim slučajevima → pusti
  next()
})

export default router