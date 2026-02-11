<style src="../assets/login.css"></style>

<script setup>
import { ref } from 'vue'
import api from '../api/axios'
import { useRouter } from 'vue-router'

const email = ref('')
const password = ref('')
const error = ref('')
const router = useRouter() // Omogucava programsku navigaciju (router.push('/home'))

const login = async () => {
  error.value = '' // reset greske ako je prethodno postojala, obrisace se 

  try {
    const res = await api.post('/auth/login', {
      email: email.value,
      password: password.value
    })

    // cuvanje tokena
    localStorage.setItem('token', res.data.token)
    localStorage.setItem('role', res.data.role)
    localStorage.setItem('userId', res.data.userId)
    localStorage.setItem('username', res.data.username)

    // redirekcija
    router.push('/home')
  } catch (err) {
    if (err.response && err.response.data && err.response.data.message) {
      error.value = err.response.data.message
    } else {
      error.value = 'Greška pri logovanju'
    }
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

      <!-- PORUKA O GREŠCI -->
      <p v-if="error" class="error-message">
        {{ error }}
      </p>

      <p class="register">
        Nemate nalog?
        <span @click="$router.push('/register')">
          Registrujte se ovde
        </span>
      </p>
    </div>
  </div>
</template>
