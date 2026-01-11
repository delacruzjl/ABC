import React, { useState } from "react"
import {
  BrowserRouter as Router,
  Routes,
  Route,
  Navigate,
  useNavigate,
} from "react-router-dom"
import { AntecedentManager } from "./components/AntecedentManager"
import { BehaviorManager } from "./components/BehaviorManager"
import { ConsequenceManager } from "./components/ConsequenceManager"
import { AntecedentList } from "./components/AntecedentList"
import { BehaviorList } from "./components/BehaviorList"
import { ConsequenceList } from "./components/ConsequenceList"
import type { Antecedent } from "./types/antecedent"
import type { Behavior } from "./types/behavior"
import type { Consequence } from "./types/consequence"

function AppRoutes() {
  const [antecedents, setAntecedents] = useState<Antecedent[]>([])
  const [behaviors, setBehaviors] = useState<Behavior[]>([])
  const [consequences, setConsequences] = useState<Consequence[]>([])

  const [editAntecedentId, setEditAntecedentId] = useState<string | null>(null)
  const [editBehaviorId, setEditBehaviorId] = useState<string | null>(null)
  const [editConsequenceId, setEditConsequenceId] = useState<string | null>(
    null
  )

  const navigate = useNavigate()

  // List navigation helpers
  const ListNav = () => (
    <nav className="bg-slate-800 border-b border-slate-700 shadow-lg mb-8">
      <div className="max-w-5xl mx-auto px-4">
        <div className="flex h-16 items-center justify-between">
          <div className="flex items-center gap-8">
            <span className="font-bold text-xl text-cyan-400 tracking-tight">
              ABC Management
            </span>
            <div className="flex gap-2">
              <button
                onClick={() => navigate("/antecedents")}
                className="text-slate-100 hover:text-cyan-400 px-3 py-2 rounded-lg transition font-medium"
              >
                Antecedents
              </button>
              <button
                onClick={() => navigate("/behaviors")}
                className="text-slate-100 hover:text-cyan-400 px-3 py-2 rounded-lg transition font-medium"
              >
                Behaviors
              </button>
              <button
                onClick={() => navigate("/consequences")}
                className="text-slate-100 hover:text-cyan-400 px-3 py-2 rounded-lg transition font-medium"
              >
                Consequences
              </button>
            </div>
          </div>
        </div>
      </div>
    </nav>
  )

  // List handlers with navigation
  const handleAddAntecedent = () => {
    setEditAntecedentId("new")
    navigate("/antecedent/manage")
  }
  const handleEditAntecedent = (id: string) => {
    setEditAntecedentId(id)
    navigate("/antecedent/manage")
  }
  const handleDeleteAntecedent = (id: string) => {
    setAntecedents(antecedents.filter((a) => a.id !== id))
    navigate("/antecedents")
  }

  const handleAddBehavior = () => {
    setEditBehaviorId("new")
    navigate("/behavior/manage")
  }
  const handleEditBehavior = (id: string) => {
    setEditBehaviorId(id)
    navigate("/behavior/manage")
  }
  const handleDeleteBehavior = (id: string) => {
    setBehaviors(behaviors.filter((b) => b.id !== id))
    navigate("/behaviors")
  }

  const handleAddConsequence = () => {
    setEditConsequenceId("new")
    navigate("/consequence/manage")
  }
  const handleEditConsequence = (id: string) => {
    setEditConsequenceId(id)
    navigate("/consequence/manage")
  }
  const handleDeleteConsequence = (id: string) => {
    setConsequences(consequences.filter((c) => c.id !== id))
    navigate("/consequences")
  }

  // Manager save handlers
  const handleSaveAntecedent = (ant: Antecedent) => {
    if (editAntecedentId === "new") {
      setAntecedents([...antecedents, { ...ant, id: Date.now().toString() }])
    } else {
      setAntecedents(
        antecedents.map((a) => (a.id === editAntecedentId ? ant : a))
      )
    }
    setEditAntecedentId(null)
    navigate("/antecedents")
  }
  const handleSaveBehavior = (beh: Behavior) => {
    if (editBehaviorId === "new") {
      setBehaviors([...behaviors, { ...beh, id: Date.now().toString() }])
    } else {
      setBehaviors(behaviors.map((b) => (b.id === editBehaviorId ? beh : b)))
    }
    setEditBehaviorId(null)
    navigate("/behaviors")
  }
  const handleSaveConsequence = (cons: Consequence) => {
    if (editConsequenceId === "new") {
      setConsequences([...consequences, { ...cons, id: Date.now().toString() }])
    } else {
      setConsequences(
        consequences.map((c) => (c.id === editConsequenceId ? cons : c))
      )
    }
    setEditConsequenceId(null)
    navigate("/consequences")
  }

  return (
    <div className="min-h-screen bg-slate-900">
      <ListNav />
      <main className="max-w-5xl mx-auto px-4 py-8">
        <div className="bg-slate-800 rounded-xl shadow-lg p-6 border border-slate-800">
          <Routes>
            <Route
              path="/antecedents"
              element={
                <AntecedentList
                  antecedents={antecedents}
                  onAdd={handleAddAntecedent}
                  onEdit={handleEditAntecedent}
                  onDelete={handleDeleteAntecedent}
                />
              }
            />
            <Route
              path="/behaviors"
              element={
                <BehaviorList
                  behaviors={behaviors}
                  onAdd={handleAddBehavior}
                  onEdit={handleEditBehavior}
                  onDelete={handleDeleteBehavior}
                />
              }
            />
            <Route
              path="/consequences"
              element={
                <ConsequenceList
                  consequences={consequences}
                  onAdd={handleAddConsequence}
                  onEdit={handleEditConsequence}
                  onDelete={handleDeleteConsequence}
                />
              }
            />
            <Route
              path="/antecedent/manage"
              element={
                editAntecedentId && (
                  <AntecedentManager
                    antecedent={
                      editAntecedentId === "new"
                        ? null
                        : antecedents.find((a) => a.id === editAntecedentId)
                    }
                    onSave={handleSaveAntecedent}
                    onCancel={() => {
                      setEditAntecedentId(null)
                      navigate("/antecedents")
                    }}
                  />
                )
              }
            />
            <Route
              path="/behavior/manage"
              element={
                editBehaviorId && (
                  <BehaviorManager
                    behavior={
                      editBehaviorId === "new"
                        ? null
                        : behaviors.find((b) => b.id === editBehaviorId)
                    }
                    onSave={handleSaveBehavior}
                    onCancel={() => {
                      setEditBehaviorId(null)
                      navigate("/behaviors")
                    }}
                  />
                )
              }
            />
            <Route
              path="/consequence/manage"
              element={
                editConsequenceId && (
                  <ConsequenceManager
                    consequence={
                      editConsequenceId === "new"
                        ? null
                        : consequences.find((c) => c.id === editConsequenceId)
                    }
                    onSave={handleSaveConsequence}
                    onCancel={() => {
                      setEditConsequenceId(null)
                      navigate("/consequences")
                    }}
                  />
                )
              }
            />
            <Route path="*" element={<Navigate to="/antecedents" replace />} />
          </Routes>
        </div>
      </main>
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
