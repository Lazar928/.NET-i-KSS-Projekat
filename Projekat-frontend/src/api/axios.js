import axios from "axios"; // Axios biblioteka za HTTP zahteve

const api = axios.create({
  baseURL: "http://localhost:5097/api", // backend projekta
});

api.interceptors.request.use((config) => { // Presretanje svakog requsta pre odlaska ka serveru
  const token = localStorage.getItem("token");
  if (token) {
    config.headers.Authorization = 'Bearer ' + token; // PRovera da li postoj JWT token i uzima ga i omogucava back-endu da ga procita
  }
  return config;
});

export default api;
