/** @type {import('next').NextConfig} */
const nextConfig = {

  reactStrictMode: false,
  async rewrites() {

    return [
      {
        source: "/admin/:path*",
        destination: "http://localhost:5256/api/" + ":path*",
      },
      {
        source: "/:path*",
        destination: "http://localhost:5256/api/" + ":path*",
      }
    ]
  },

}

module.exports = nextConfig
