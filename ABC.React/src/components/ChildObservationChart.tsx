import React from "react"
import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
  Legend,
} from "recharts"
import type { ItemWithChildData } from "../hooks/useDashboard"

interface Props {
  title: string
  items: ItemWithChildData[]
  barColor?: string
}

export const ChildObservationChart: React.FC<Props> = ({
  title,
  items,
  barColor = "#22d3ee",
}) => {
  if (items.length === 0 || items.every((i) => i.childCount === 0)) {
    return (
      <div className="bg-slate-700 rounded-lg p-4">
        <h3 className="text-lg font-semibold text-cyan-400 mb-3">{title}</h3>
        <p className="text-slate-400 text-sm">No child data available</p>
      </div>
    )
  }

  // Build data: each item becomes a group showing children count + observation count
  const chartData = items
    .filter((item) => item.childCount > 0)
    .map((item) => ({
      name:
        item.name.length > 15 ? item.name.slice(0, 15) + "…" : item.name,
      fullName: item.name,
      children: item.childCount,
      observations: item.observationCount,
    }))

  return (
    <div className="bg-slate-700 rounded-lg p-4">
      <h3 className="text-lg font-semibold text-cyan-400 mb-3">{title}</h3>
      <ResponsiveContainer width="100%" height={300}>
        <BarChart
          data={chartData}
          margin={{ top: 5, right: 20, left: 0, bottom: 60 }}
        >
          <CartesianGrid strokeDasharray="3 3" stroke="#475569" />
          <XAxis
            dataKey="name"
            tick={{ fill: "#cbd5e1", fontSize: 11 }}
            angle={-45}
            textAnchor="end"
            height={80}
          />
          <YAxis
            tick={{ fill: "#cbd5e1", fontSize: 12 }}
            allowDecimals={false}
          />
          <Tooltip
            contentStyle={{
              backgroundColor: "#1e293b",
              border: "1px solid #475569",
              borderRadius: "8px",
              color: "#e2e8f0",
            }}
            labelFormatter={(_label, payload) =>
              payload?.[0]?.payload?.fullName ?? _label
            }
          />
          <Legend
            wrapperStyle={{ color: "#cbd5e1", fontSize: 12 }}
          />
          <Bar
            dataKey="children"
            name="Children"
            fill="#a78bfa"
            radius={[4, 4, 0, 0]}
          />
          <Bar
            dataKey="observations"
            name="Observations"
            fill={barColor}
            radius={[4, 4, 0, 0]}
          />
        </BarChart>
      </ResponsiveContainer>
    </div>
  )
}
