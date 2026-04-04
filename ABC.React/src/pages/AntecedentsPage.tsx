import React from "react"
import { useNavigate } from "react-router-dom"
import { useAntecedents } from "../hooks/useAntecedents"
import { AntecedentList } from "../components/AntecedentList"

export const AntecedentsPage: React.FC = () => {
  const { antecedents, loading, error, removeAntecedent } = useAntecedents()
  const navigate = useNavigate()

  if (loading) {
    return (
      <div className="flex justify-center py-12">
        <span className="text-cyan-400 text-lg">Loading antecedents…</span>
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
    <AntecedentList
      antecedents={antecedents}
      onAdd={() => navigate("/antecedent/manage")}
      onEdit={(id) => navigate(`/antecedent/manage?id=${id}`)}
      onDelete={(id) => removeAntecedent(id)}
    />
  )
}
