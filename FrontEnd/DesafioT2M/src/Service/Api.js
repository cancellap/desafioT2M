import axios from "axios";

const api = axios.create({
    baseURL: "http://localhost:5029/api",
});

export const getUsuario = async (id) => {
    try {
        const response = await api.get(`/usuario/${id}`);
        return response.data;
    } catch (error) {
        console.error("Erro ao buscar usuário:", error);
        throw error;
    }
};

export default api;
