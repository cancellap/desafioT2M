import React from "react";
import { Routes, Route } from "react-router-dom";
import Login from "../Pages/Login/Login";
import Cadastro from "../Pages/Cadastro/Cadastro";
import ProjetosGerais from "../Pages/ProjetosGerais/ProjetosGerais";
import ProjetosPropios from "../Pages/ProjetosPropios/ProjetosPropios";
import CreateProjeto from "../Pages/CreateProjeto/CreateProjeto";
import ProtectedRoute from "./RouteProtegida";
import DetalhesProjeto from "../Pages/DetalhesProjeto/DetalhesProjeto";

export default function AppRoute() {
  const auth = localStorage.getItem("autenticado");
  const isAuthenticated = auth;

  return (
    <Routes>
      <Route path="/login" element={<Login />} />
      <Route path="/cadastro" element={<Cadastro />} />
      <Route
        path="/projetosGerais"
        element={
          <ProtectedRoute isAuthenticated={isAuthenticated}>
            <ProjetosGerais />
          </ProtectedRoute>
        }
      />
      <Route
        path="/projetosPropios"
        element={
          <ProtectedRoute isAuthenticated={isAuthenticated}>
            <ProjetosPropios />
          </ProtectedRoute>
        }
      />
      <Route
        path="/createProjeto"
        element={
          <ProtectedRoute isAuthenticated={isAuthenticated}>
            <CreateProjeto />
          </ProtectedRoute>
        }
      />
      <Route
        path="/projeto/:id"
        element={
          <ProtectedRoute isAuthenticated={isAuthenticated}>
            <DetalhesProjeto />
          </ProtectedRoute>
        }
      />
    </Routes>
  );
}
