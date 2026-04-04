import React from "react"
import { useNavigate } from "react-router-dom"
import { useConsequences } from "../hooks/useConsequences"
import { ConsequenceList } from "../components/ConsequenceList"

export const ConsequencesPage: React.FC = () => {
  const { consequences, loading, error, removeConsequence } = useConsequences()
  const navigate = useNavigate()

  if (loading) {
    return (
      <div className="flex justify-center py-12">
        <span className="text-cyan-400 text-lg">Loading consequences…</span>
      </div>
    )
  }

  if (error) {
    return (
      <div className="flex justify-center py-12">
        <span className="text-red-400 text-lg">Error: {error.message}</span>
      </div>
    )
  }

  return (
    <ConsequenceList
      consequences={consequences}
      onAdd={() => navigate("/consequence/manage")}
      onEdit={(id) => navigate(`/consequence/manage?id=${id}`)}
      onDelete={(id) => removeConsequence(id)}
    />
  )
}
