<style src="../assets/register.css"></style>

<template>
  <div class="auth-page">
    <div class="auth-card">
      <h1 class="brand">AutoPlac</h1>
      <hr />

      <h2 class="title">Registracija</h2>

      <label>Username</label>
      <input type="text" v-model="username" />

      <label>Email</label>
      <input type="email" v-model="email" />

      <label>Password</label>
      <input type="password" v-model="password" />

      <label>Role</label>

      <div class="role-box" :class="{ active: role === 'Buyer' }"
           @click="role = 'Buyer'">
        <span class="dot"></span>
        Buyer
      </div>

      <div class="role-box" :class="{ active: role === 'Owner' }"
           @click="role = 'Owner'">
           <span class="dot"></span>
        Owner 
      </div>

      <button @click="register">Registracija</button>

      <p class="switch">
        Da li već imate nalog?
        <span @click="$router.push('/login')">Ulogujte se ovde</span>
      </p>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import api from '../api/axios'
import { useRouter } from 'vue-router'

const username = ref('')
const email = ref('')
const password = ref('')
const role = ref('Buyer') // default Kupac
const error = ref('')
const router = useRouter()

const register = async () => {
  error.value = '' // reset greske ako je prethodno postojala, obrisace se

  if (!username.value || !email.value || !password.value || !role.value){
    error.value = 'Popunite sva polja'
    return
  }

  try {
    await api.post('/auth/register', {
      username: username.value,
      email: email.value,
      password: password.value,
      role: role.value // User ili Owner
    })

    router.push('/login')
  } catch (err) {
    error.value = err.response?.data || 'Greška pri registraciji'
  }
}
</script>

