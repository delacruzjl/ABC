import React from "react"
import {
  BrowserRouter as Router,
  Routes,
  Route,
  Navigate,
  useNavigate,
  useLocation,
} from "react-router-dom"
import { AntecedentsPage } from "./pages/AntecedentsPage"
import { BehaviorsPage } from "./pages/BehaviorsPage"
import { ConsequencesPage } from "./pages/ConsequencesPage"
import { AntecedentFormPage } from "./pages/AntecedentFormPage"
import { BehaviorFormPage } from "./pages/BehaviorFormPage"
import { ConsequenceFormPage } from "./pages/ConsequenceFormPage"
import { DashboardPage } from "./pages/DashboardPage"
import { LoginPage } from "./pages/LoginPage"
import { RegisterPage } from "./pages/RegisterPage"
import { ChildrenPage } from "./pages/ChildrenPage"
import { ChildFormPage } from "./pages/ChildFormPage"
import { ProtectedRoute } from "./components/ProtectedRoute"
import { useAuth } from "./context/AuthContext"

function NavBar() {
  const navigate = useNavigate()
  const location = useLocation()
  const { user, isAdmin, isAuthenticated, logout } = useAuth()

  const isActive = (path: string) =>
    location.pathname === path || location.pathname.startsWith(path + "/")

  const navButtonClass = (path: string) =>
    `px-3 py-2 rounded-lg transition font-medium ${
      isActive(path)
        ? "text-cyan-400 bg-slate-700"
        : "text-slate-100 hover:text-cyan-400"
    }`

  if (!isAuthenticated) return null

  return (
    <nav className="bg-slate-800 border-b border-slate-700 shadow-lg mb-8">
      <div className="max-w-5xl mx-auto px-4">
        <div className="flex h-16 items-center justify-between">
          <div className="flex items-center gap-8">
            <button
              onClick={() => navigate("/")}
              className="font-bold text-xl text-cyan-400 tracking-tight hover:text-cyan-300 transition"
            >
              ABC Management
            </button>
            <div className="flex gap-2">
              <button
                onClick={() => navigate("/")}
                className={navButtonClass("/")}
              >
                Dashboard
              </button>
              <button
                onClick={() => navigate("/children")}
                className={navButtonClass("/children")}
              >
                Children
              </button>
              {isAdmin && (
                <>
                  <button
                    onClick={() => navigate("/antecedents")}
                    className={navButtonClass("/antecedents")}
                  >
                    Antecedents
                  </button>
                  <button
                    onClick={() => navigate("/behaviors")}
                    className={navButtonClass("/behaviors")}
                  >
                    Behaviors
                  </button>
                  <button
                    onClick={() => navigate("/consequences")}
                    className={navButtonClass("/consequences")}
                  >
                    Consequences
                  </button>
                </>
              )}
            </div>
          </div>
          <div className="flex items-center gap-4">
            <span className="text-slate-400 text-sm">
              {user?.email}
              {isAdmin && (
                <span className="ml-2 text-xs bg-amber-900 text-amber-300 px-2 py-0.5 rounded">
                  Admin
                </span>
              )}
            </span>
            <button
              onClick={() => {
                logout()
                navigate("/login")
              }}
              className="text-slate-400 hover:text-red-400 text-sm font-medium transition"
            >
              Logout
            </button>
          </div>
        </div>
      </div>
    </nav>
  )
}

import { OfflineBanner } from "./components/OfflineBanner"

function AppRoutes() {
  return (
    <div className="min-h-screen bg-slate-900">
      <OfflineBanner />
      <NavBar />
      <Routes>
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route
          path="/"
          element={
            <ProtectedRoute>
              <main className="max-w-5xl mx-auto px-4 py-8">
                <div className="bg-slate-800 rounded-xl shadow-lg p-6 border border-slate-800">
                  <DashboardPage />
                </div>
              </main>
            </ProtectedRoute>
          }
        />
        <Route
          path="/children"
          element={
            <ProtectedRoute>
              <main className="max-w-5xl mx-auto px-4 py-8">
                <div className="bg-slate-800 rounded-xl shadow-lg p-6 border border-slate-800">
                  <ChildrenPage />
                </div>
              </main>
            </ProtectedRoute>
          }
        />
        <Route
          path="/child/manage"
          element={
            <ProtectedRoute>
              <main className="max-w-5xl mx-auto px-4 py-8">
                <div className="bg-slate-800 rounded-xl shadow-lg p-6 border border-slate-800">
                  <ChildFormPage />
                </div>
              </main>
            </ProtectedRoute>
          }
        />
        <Route
          path="/child/edit/:id"
          element={
            <ProtectedRoute>
              <main className="max-w-5xl mx-auto px-4 py-8">
                <div className="bg-slate-800 rounded-xl shadow-lg p-6 border border-slate-800">
                  <ChildFormPage />
                </div>
              </main>
            </ProtectedRoute>
          }
        />
        <Route
          path="/antecedents"
          element={
            <ProtectedRoute>
              <main className="max-w-5xl mx-auto px-4 py-8">
                <div className="bg-slate-800 rounded-xl shadow-lg p-6 border border-slate-800">
                  <AntecedentsPage />
                </div>
              </main>
            </ProtectedRoute>
          }
        />
        <Route
          path="/behaviors"
          element={
            <ProtectedRoute>
              <main className="max-w-5xl mx-auto px-4 py-8">
                <div className="bg-slate-800 rounded-xl shadow-lg p-6 border border-slate-800">
                  <BehaviorsPage />
                </div>
              </main>
            </ProtectedRoute>
          }
        />
        <Route
          path="/consequences"
          element={
            <ProtectedRoute>
              <main className="max-w-5xl mx-auto px-4 py-8">
                <div className="bg-slate-800 rounded-xl shadow-lg p-6 border border-slate-800">
                  <ConsequencesPage />
                </div>
              </main>
            </ProtectedRoute>
          }
        />
        <Route
          path="/antecedent/manage"
          element={
            <ProtectedRoute>
              <main className="max-w-5xl mx-auto px-4 py-8">
                <div className="bg-slate-800 rounded-xl shadow-lg p-6 border border-slate-800">
                  <AntecedentFormPage />
                </div>
              </main>
            </ProtectedRoute>
          }
        />
        <Route
          path="/behavior/manage"
          element={
            <ProtectedRoute>
              <main className="max-w-5xl mx-auto px-4 py-8">
                <div className="bg-slate-800 rounded-xl shadow-lg p-6 border border-slate-800">
                  <BehaviorFormPage />
                </div>
              </main>
            </ProtectedRoute>
          }
        />
        <Route
          path="/consequence/manage"
          element={
            <ProtectedRoute>
              <main className="max-w-5xl mx-auto px-4 py-8">
                <div className="bg-slate-800 rounded-xl shadow-lg p-6 border border-slate-800">
                  <ConsequenceFormPage />
                </div>
              </main>
            </ProtectedRoute>
          }
        />
        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </div>
  )
}

function App() {
  return (
    <Router>
      <AppRoutes />
    </Router>
  )
}

export default App
