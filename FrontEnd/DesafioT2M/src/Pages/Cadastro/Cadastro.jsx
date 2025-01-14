import React from "react";
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import * as Yup from "yup";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import styles from "./Cadastro.module.css";

const checkUsername = async (username) => {
  try {
    const response = await axios.post(`http://localhost:5029/api/usuario/username/${username}`);
    return response.data ? true : false; 
  } catch {
    return false; 
  }
};

const schema = Yup.object().shape({
  nome: Yup.string().required("Nome é obrigatório"),
  cargo: Yup.string().required("Cargo é obrigatório"),
  username: Yup.string()
    .required("Username é obrigatório")
    .test("check-username", "Username já existe", async (value) => {
      if (!value) return true; 
      const exists = await checkUsername(value);
      return !exists; 
      //queria que mostrasse para na tela que o usuario ja existe porem nao deu tempo
    }),
  password: Yup.string().required("Senha é obrigatória"),
  passwordConfirm: Yup.string()
    .oneOf([Yup.ref("password"), null], "As senhas não coincidem")
    .required("Confirmação de senha é obrigatória"),
});

export default function FormCadastro() {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm({
    resolver: yupResolver(schema),
  });

  const navigate = useNavigate();

  const onSubmit = async (data) => {
    try {
      const payload = { ...data, tarefas: [] };
      const response = await axios.post("http://localhost:5029/api/usuario", payload);
      console.log("Cadastro bem-sucedido", response.data);
      navigate("/login");
    } catch (error) {
      console.error("Erro no cadastro", error);
    }
  };

  const handleRedirect = () => {
    navigate("/login");
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)} className={styles.formContainer}>
      <div>
        <label htmlFor="nome" className={styles.label}>
          Nome
        </label>
        <input
          id="nome"
          type="text"
          className={`${styles.input} ${errors.nome ? styles.inputError : ""}`}
          {...register("nome")}
        />
        {errors.nome && <p className={styles.errorMessage}>{errors.nome.message}</p>}
      </div>

      <div>
        <label htmlFor="cargo" className={styles.label}>
          Cargo
        </label>
        <input
          id="cargo"
          type="text"
          className={`${styles.input} ${errors.cargo ? styles.inputError : ""}`}
          {...register("cargo")}
        />
        {errors.cargo && <p className={styles.errorMessage}>{errors.cargo.message}</p>}
      </div>

      <div>
        <label htmlFor="username" className={styles.label}>
          Username
        </label>
        <input
          id="username"
          type="text"
          className={`${styles.input} ${errors.username ? styles.inputError : ""}`}
          {...register("username")}
        />
        {errors.username && (
          <p className={styles.errorMessage}>{errors.username.message}</p>
        )}
      </div>

      <div>
        <label htmlFor="password" className={styles.label}>
          Senha
        </label>
        <input
          id="password"
          type="password"
          className={`${styles.input} ${errors.password ? styles.inputError : ""}`}
          {...register("password")}
        />
        {errors.password && (
          <p className={styles.errorMessage}>{errors.password.message}</p>
        )}
      </div>

      <div>
        <label htmlFor="passwordConfirm" className={styles.label}>
          Confirme sua senha
        </label>
        <input
          id="passwordConfirm"
          type="password"
          className={`${styles.input} ${errors.passwordConfirm ? styles.inputError : ""}`}
          {...register("passwordConfirm")}
        />
        {errors.passwordConfirm && (
          <p className={styles.errorMessage}>{errors.passwordConfirm.message}</p>
        )}
      </div>

      <button
        type="button"
        onClick={handleRedirect}
        className={styles.redirectButton}
      >
        <p>Já tem uma conta? Faça login</p>
      </button>

      <button type="submit" className={styles.button}>
        Cadastro
      </button>
    </form>
  );
}
