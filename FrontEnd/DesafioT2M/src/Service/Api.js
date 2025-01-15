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

export const addTarefa = async (novaTarefa, token) => {
    try {
        const response = await axios.post(
            "http://localhost:5029/api/tarefa",
            novaTarefa,
            {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            }
        );
        return response.data;
    } catch (error) {
        console.error("Erro ao salvar a tarefa:", error);
        throw error;
    }
};

export const updateProjeto = async (id, updatedProjeto, token) => {
    try {
        const response = await axios.put(
            `http://localhost:5029/api/projeto/${id}`,
            updatedProjeto,
            {
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${token}`,
                },
            }
        );
        return response.data;
    } catch (error) {
        console.error("Erro ao atualizar o projeto:", error);
        throw error;
    }
};

export const updateTarefa = async (id, updatedTarefa, token) => {
    try {
        const response = await api.put(`/tarefa/${id}`, updatedTarefa, {
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${token}`,
            },
        });
        return response.data;
    } catch (error) {
        console.error("Erro ao atualizar tarefa:", error);
        throw error;
    }
};

export const checkUsername = async (username) => {
    try {
        const response = await api.post(`/usuario/username/${username}`);
        return response.data ? true : false;
    } catch {
        return false;
    }
};

export const addUsuario = async (usuario) => {
    try {
        const response = await api.post("/usuario", usuario);
        return response.data;
    } catch (error) {
        console.error("Erro ao cadastrar usuário:", error);
        throw error;
    }
};

export const postProjeto = async (projeto) => {
    const token = localStorage.getItem("token");

    if (!token) {
        console.error("Token não encontrado. Faça login novamente.");
        return;
    }

    try {
        const response = await api.post("/projeto", projeto, {
            headers: { Authorization: `Bearer ${token}` },
        });
        console.log("Projeto criado com sucesso:", response.data);
        return response.data;
    } catch (error) {
        console.error("Erro ao criar projeto:", error.message);
        if (error.response) {
            console.error("Detalhes do erro:", error.response.data);
        }
        throw new Error(
            "Erro ao criar projeto. Verifique os dados e tente novamente."
        );
    }
};

export const fetchProjeto = async (id) => {
    try {
        const response = await api.get(`/projeto/${id}`);
        const usuarioData = await getUsuario(response.data.usuarioId);
        const tarefasComUsuarios = await Promise.all(
            response.data.tarefas.map(async (tarefa) => {
                const usuarioData = await getUsuario(tarefa.usuarioId);
                return { ...tarefa, usuarioNome: usuarioData.nome };
            })
        );
        return {
            projeto: response.data,
            usuarioNome: usuarioData.nome,
            tarefasComUsuarios,
        };
    } catch (error) {
        throw new Error(error.message);
    }
};

export const createTarefa = async (novaTarefa) => {
    try {
        await api.post("/tarefa", novaTarefa, {
            headers: { "Content-Type": "application/json" },
        });
    } catch (error) {
        throw new Error(error.message);
    }
};

export const deleteTarefa = async (idTarefa, token) => {
    try {
        await api.delete(`/tarefa/${idTarefa}`, {
            headers: { Authorization: `Bearer ${token}` },
        });
    } catch (error) {
        throw new Error(error.message);
    }
};

export const deleteProjeto = async (idTarefa, token) => {
    try {
        await api.delete(`/projeto/${idTarefa}`, {
            headers: { Authorization: `Bearer ${token}` },
        });
    } catch (error) {
        throw new Error(error.message);
    }
};

export const editProjeto = async (id, projeto) => {
    try {
        const response = await api.put(`/projeto/${id}`, projeto, {
            headers: { "Content-Type": "application/json" },
        });
        return response.data;
    } catch (error) {
        throw new Error(error.message);
    }
};

export const loginUser = async (data) => {
    try {
        const response = await api.post("/login", data);
        return response.data;
    } catch (error) {
        console.error("Erro no login", error);
        throw error;
    }
};

export const getProjetos = async () => {
    try {
        const response = await api.get("/projeto");
        return response.data;
    } catch (error) {
        console.error("Erro ao buscar projetos:", error);
        throw error;
    }
};

export const getProjetosPorUsuario = async (token) => {
    try {
        const response = await api.get("/projeto/porUsuario", {
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${token}`,
            },
        });
        return response.data;
    } catch (error) {
        console.error("Erro ao buscar projetos do usuário:", error);
        throw error;
    }
};
export default api;
