<style src="../assets/login.css"></style>

<script setup>
import { ref } from 'vue'
import api from '../api/axios'
import { useRouter } from 'vue-router'

const email = ref('')
const password = ref('')
const error = ref('')
const router = useRouter()

const login = async () => {
  error.value = ''

  try {
    const res = await api.post('/auth/login', {
      email: email.value,
      password: password.value
    })

    // backend vraca token
    localStorage.setItem('token', res.data.token)

    // opciono: sacuvaj rolu
    localStorage.setItem('role', res.data.role)

    // pamti koji user je log in-ovan
    localStorage.setItem('userId', res.data.userId)

    // pamti naziv usera/owner/admina koji je ulogovan
    localStorage.setItem('username', res.data.username)

    // redirekcija posle logina
    router.push('/home')
  } catch (err) {
    error.value = err.response?.data || 'Greška pri logovanju'
  }
}
</script>

<template>
  <div class="login-page">
    <div class="login-card">
      <h1 class="brand">AutoPlac</h1>
      <hr />

      <h2 class="title">Prijava</h2>

      <label>Email</label>
      <input type="email" v-model="email" />

      <label>Password</label>
      <input type="password" v-model="password" />

      <button @click="login">Prijava</button>

      <p class="register">
        Nemate nalog?
        <span @click="$router.push('/register')">Registrujte se ovde</span>
      </p>
    </div>
  </div>
</template>

