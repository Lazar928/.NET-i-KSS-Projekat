<template>
  <div v-if="visible" class="toast">
    {{ message }}
  </div>
</template>

<script setup>
import { ref } from 'vue'

const visible = ref(false)
const message = ref('')

const showToast = (msg, duration = 7000) => {
  message.value = msg
  visible.value = true

  setTimeout(() => {
    visible.value = false
  }, duration)
}

// ⬇️ omogućava da roditelj pozove showToast
defineExpose({ showToast })
</script>

<style scoped>
.toast {
  position: fixed;
  top: 20px;
  right: 20px;
  background-color: #22c55e; /* zelena */
  color: white;
  padding: 15px 20px;
  border-radius: 6px;
  font-weight: 500;
  box-shadow: 0 4px 10px rgba(0,0,0,0.2);
  z-index: 9999;
}
</style>