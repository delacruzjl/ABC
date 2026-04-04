import React from "react"
import { useNavigate } from "react-router-dom"
import { useDashboard } from "../hooks/useDashboard"
import { TopItemsChart } from "../components/TopItemsChart"
import { ChildObservationChart } from "../components/ChildObservationChart"
import { RecentObservationsList } from "../components/RecentObservationsList"
import { useAuth } from "../context/AuthContext"

export const DashboardPage: React.FC = () => {
  const {
    topAntecedents,
    topBehaviors,
    topConsequences,
    recentObservations,
    loading,
    error,
  } = useDashboard()
  const navigate = useNavigate()
  const { isAdmin } = useAuth()

  return (
    <div>
      <div className="mb-8">
        <h1 className="text-2xl font-bold text-cyan-400 mb-2">Dashboard</h1>
        <p className="text-slate-400">
          Overview of antecedents, behaviors, and consequences by observation
          frequency.
        </p>
      </div>

      {/* Quick navigation cards */}
      <div className="grid grid-cols-1 md:grid-cols-4 gap-4 mb-8">
        <button
          onClick={() => navigate("/children")}
          className="bg-slate-700 hover:bg-slate-600 rounded-lg p-4 text-left transition border border-slate-600 hover:border-cyan-400"
        >
          <h3 className="text-lg font-semibold text-cyan-400">Children</h3>
          <p className="text-slate-400 text-sm mt-1">
            Manage children & view observations
          </p>
        </button>
        {isAdmin && (
          <>
            <button
              onClick={() => navigate("/antecedents")}
              className="bg-slate-700 hover:bg-slate-600 rounded-lg p-4 text-left transition border border-slate-600 hover:border-cyan-400"
            >
              <h3 className="text-lg font-semibold text-cyan-400">
                Antecedents
              </h3>
              <p className="text-slate-400 text-sm mt-1">
                Manage antecedent options
              </p>
            </button>
            <button
              onClick={() => navigate("/behaviors")}
              className="bg-slate-700 hover:bg-slate-600 rounded-lg p-4 text-left transition border border-slate-600 hover:border-cyan-400"
            >
              <h3 className="text-lg font-semibold text-cyan-400">
                Behaviors
              </h3>
              <p className="text-slate-400 text-sm mt-1">
                Manage behavior options
              </p>
            </button>
            <button
              onClick={() => navigate("/consequences")}
              className="bg-slate-700 hover:bg-slate-600 rounded-lg p-4 text-left transition border border-slate-600 hover:border-cyan-400"
            >
              <h3 className="text-lg font-semibold text-cyan-400">
                Consequences
              </h3>
              <p className="text-slate-400 text-sm mt-1">
                Manage consequence options
              </p>
            </button>
          </>
        )}
      </div>

      {loading && (
        <div className="flex justify-center py-12">
          <span className="text-cyan-400 text-lg">Loading dashboard…</span>
        </div>
      )}

      {error && (
        <div className="flex justify-center py-4 mb-4">
          <span className="text-red-400 text-sm">
            Error loading data: {error.message}
          </span>
        </div>
      )}

      {!loading && (
        <>
          {/* Top 10 charts */}
          <h2 className="text-xl font-bold text-slate-200 mb-4">
            Top 10 by Observation Count
          </h2>
          <div className="grid grid-cols-1 lg:grid-cols-3 gap-4 mb-8">
            <TopItemsChart
              title="Top Antecedents"
              items={topAntecedents}
              barColor="#22d3ee"
            />
            <TopItemsChart
              title="Top Behaviors"
              items={topBehaviors}
              barColor="#a78bfa"
            />
            <TopItemsChart
              title="Top Consequences"
              items={topConsequences}
              barColor="#fbbf24"
            />
          </div>

          {/* Children & Observations breakdown */}
          <h2 className="text-xl font-bold text-slate-200 mb-4">
            Children &amp; Observations per Item
          </h2>
          <div className="grid grid-cols-1 lg:grid-cols-3 gap-4 mb-8">
            <ChildObservationChart
              title="Antecedents"
              items={topAntecedents}
              barColor="#22d3ee"
            />
            <ChildObservationChart
              title="Behaviors"
              items={topBehaviors}
              barColor="#a78bfa"
            />
            <ChildObservationChart
              title="Consequences"
              items={topConsequences}
              barColor="#fbbf24"
            />
          </div>

          {/* Recent observations */}
          <h2 className="text-xl font-bold text-slate-200 mb-4">
            3 Most Recent Observations
          </h2>
          <RecentObservationsList observations={recentObservations} />
        </>
      )}
    </div>
  )
}
