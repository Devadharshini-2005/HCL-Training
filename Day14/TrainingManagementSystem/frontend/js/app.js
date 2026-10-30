const API = window.API_BASE_URL;
let selectedRole = "Student";
let currentUser = null;
let allCourses = [];

const $ = (id) => document.getElementById(id);
const money = (amount) => new Intl.NumberFormat("en-IN", { style: "currency", currency: "INR", maximumFractionDigits: 2 }).format(amount ?? 0);
const safe = (value) => String(value ?? "").replace(/[&<>"']/g, ch => ({ "&":"&amp;", "<":"&lt;", ">":"&gt;", '"':"&quot;", "'":"&#39;" }[ch]));

async function api(path, options = {}) {
  const response = await fetch(`${API}${path}`, {
    ...options,
    headers: { "Content-Type": "application/json", ...(options.headers || {}) }
  });
  const data = response.status === 204 ? null : await response.json().catch(() => ({}));
  if (!response.ok) throw new Error(data?.message || `Request failed (${response.status})`);
  return data;
}
function message(id, text, error = false) {
  const el = $(id); el.textContent = text; el.classList.toggle("error", error);
}
function roleUI() {
  document.querySelectorAll(".tab").forEach(t => t.classList.toggle("active", t.dataset.role === selectedRole));
  $("authTitle").textContent = selectedRole === "Admin" ? "Admin sign in" : "Welcome back";
  $("authSubtitle").textContent = selectedRole === "Admin" ? "Access course operations and reports." : "Sign in to your student workspace.";
  $("registerLinkWrap").classList.toggle("hidden", selectedRole === "Admin");
  $("authMessage").textContent = "";
}
document.querySelectorAll(".tab").forEach(t => t.addEventListener("click", () => { selectedRole = t.dataset.role; roleUI(); }));
$("showRegister").addEventListener("click", () => { $("loginForm").classList.add("hidden"); $("registerForm").classList.remove("hidden"); $("authMessage").textContent = ""; });
$("cancelRegister").addEventListener("click", () => { $("registerForm").classList.add("hidden"); $("loginForm").classList.remove("hidden"); });

$("loginForm").addEventListener("submit", async (event) => {
  event.preventDefault();
  try {
    currentUser = await api("/api/auth/login", { method: "POST", body: JSON.stringify({ email: $("email").value.trim(), password: $("password").value, role: selectedRole }) });
    showApp();
  } catch (err) { message("authMessage", err.message, true); }
});
$("registerForm").addEventListener("submit", async (event) => {
  event.preventDefault();
  try {
    currentUser = await api("/api/auth/register", { method: "POST", body: JSON.stringify({
      name: $("regName").value.trim(), contactNumber: $("regPhone").value.trim(),
      email: $("regEmail").value.trim(), password: $("regPassword").value
    }) });
    showApp();
  } catch (err) { message("authMessage", err.message, true); }
});
$("logoutBtn").addEventListener("click", () => {
  currentUser = null;
  $("appView").classList.add("hidden"); $("authView").classList.remove("hidden");
  $("logoutBtn").classList.add("hidden"); $("userBadge").classList.add("hidden");
  $("password").value = ""; $("authMessage").textContent = ""; window.scrollTo(0,0);
});
$("courseSearch").addEventListener("input", renderCourses);
$("courseSort").addEventListener("change", loadCourses);
$("refreshBtn").addEventListener("click", refreshCurrent);
$("courseForm").addEventListener("submit", async event => {
  event.preventDefault();
  try {
    await api("/api/courses", { method:"POST", body:JSON.stringify({
      courseCode:$("newCode").value.trim(), title:$("newTitle").value.trim(),
      fee:Number($("newFee").value), durationDays:Number($("newDuration").value), totalSeats:Number($("newSeats").value)
    })});
    event.target.reset(); message("adminMessage", "Course added successfully."); await loadCourses();
  } catch(err) { message("adminMessage", err.message, true); }
});
$("reportBtn").addEventListener("click", loadReports);

function showApp() {
  $("authView").classList.add("hidden"); $("appView").classList.remove("hidden");
  $("logoutBtn").classList.remove("hidden"); $("userBadge").classList.remove("hidden");
  $("userBadge").textContent = `${currentUser.name} · ${currentUser.role}`;
  const admin = currentUser.role === "Admin";
  $("dashboardTitle").textContent = admin ? "Training overview" : "Course catalogue";
  $("dashboardSubtitle").textContent = admin ? "Manage courses, learners and training activity." : "Find a course that moves you forward.";
  $("adminTools").classList.toggle("hidden", !admin);
  $("adminReports").classList.toggle("hidden", !admin);
  $("adminStats").classList.toggle("hidden", !admin);
  $("recordsTitle").textContent = admin ? "All enrollments" : "My enrollments";
  $("recordsHead").innerHTML = admin
    ? "<tr><th>Ref</th><th>Student</th><th>Course</th><th>Fee</th><th>Status</th><th>Payment</th><th>Action</th></tr>"
    : "<tr><th>Ref</th><th>Course</th><th>Date</th><th>Fee</th><th>Status</th><th>Payment</th><th>Action</th></tr>";
  loadCourses(); loadEnrollments();
  if (admin) loadAdminStats();
  window.scrollTo(0,0);
}
async function loadCourses() {
  try {
    const sort = $("courseSort").value;
    allCourses = await api(`/api/courses${sort ? `?sort=${sort}` : ""}`);
    renderCourses();
  } catch(err) { $("courseGrid").innerHTML = `<div class="panel">Could not load courses: ${safe(err.message)}. Check that the backend is running.</div>`; }
}
function renderCourses() {
  const search = $("courseSearch").value.trim().toLowerCase();
  const courses = allCourses.filter(c => c.title.toLowerCase().includes(search) || c.courseCode.toLowerCase().includes(search));
  $("courseGrid").innerHTML = courses.map(c => `
    <article class="course-card">
      <div class="course-top"><span class="course-code">${safe(c.courseCode)}</span><span class="seat-pill">${c.availableSeats} seats left</span></div>
      <h3>${safe(c.title)}</h3><div class="course-meta"><span>◷ ${c.durationDays} days</span><span>♙ ${c.enrolledCount}/${c.totalSeats} enrolled</span></div>
      <div class="course-bottom"><div class="price">${money(c.fee)}<small>Course fee · surcharge extra</small></div>
      ${currentUser?.role === "Student" ? `<button class="btn btn-primary enroll-btn" data-code="${safe(c.courseCode)}" ${c.availableSeats <= 0 ? "disabled" : ""}>Enroll →</button>` : `<span class="seat-pill">${c.availableSeats <= 0 ? "Full" : "Available"}</span>`}</div>
    </article>`).join("") || `<div class="panel">No courses match your search.</div>`;
  document.querySelectorAll(".enroll-btn").forEach(btn => btn.addEventListener("click", () => enroll(btn.dataset.code)));
}
async function enroll(courseCode) {
  const earlyBird = confirm("Does this enrollment qualify for the 10% early-bird discount? Choose OK for yes, Cancel for no.");
  const groupSize = Number(prompt("How many eligible learners are enrolling together? Enter 1 if individual.", "1") || "1");
  const sessionType = confirm("Choose OK for Classroom (+₹800), or Cancel for Online (+₹200).") ? "Classroom" : "Online";
  try {
    const result = await api("/api/enrollments", { method:"POST", body:JSON.stringify({ learnerId:currentUser.id, courseCode, earlyBird, groupSize, sessionType }) });
    alert(`Enrollment created!\nReference: ${result.id}\nBase fee: ${money(result.baseFee)}\nDiscount: ${money(result.discountAmount)}\nFinal fee: ${money(result.finalFee)}`);
    await loadCourses(); await loadEnrollments();
  } catch(err) { alert(err.message); }
}
async function loadEnrollments() {
  try {
    const query = currentUser.role === "Admin" ? "" : `?learnerId=${currentUser.id}`;
    const items = await api(`/api/enrollments${query}`);
    $("recordsBody").innerHTML = items.map(e => `<tr>
      <td>#${e.id}</td>${currentUser.role === "Admin" ? `<td>${safe(e.learnerName)}</td>` : ""}
      <td>${safe(e.courseTitle)} <small>(${safe(e.courseCode)})</small></td>
      ${currentUser.role === "Student" ? `<td>${new Date(e.enrollmentDate).toLocaleDateString()}</td>` : ""}
      <td>${money(e.finalFee)}</td><td><span class="status ${e.status.toLowerCase()}">${safe(e.status)}</span></td>
      <td><span class="status ${e.paymentStatus === "Paid" ? "successful" : "pending"}">${safe(e.paymentStatus)}</span></td>
      <td>${e.status === "Active" ? `${currentUser.role === "Student" ? `<button class="btn btn-outline pay-btn" data-id="${e.id}">Pay</button>` : ""} <button class="btn btn-outline cancel-btn" data-id="${e.id}">Cancel</button>` : "—"}</td>
    </tr>`).join("") || `<tr><td colspan="7">No enrollment records yet.</td></tr>`;
    document.querySelectorAll(".cancel-btn").forEach(btn => btn.addEventListener("click", () => cancelEnrollment(Number(btn.dataset.id))));
    document.querySelectorAll(".pay-btn").forEach(btn => btn.addEventListener("click", () => payEnrollment(Number(btn.dataset.id))));
  } catch(err) { $("recordsBody").innerHTML = `<tr><td colspan="7">${safe(err.message)}</td></tr>`; }
}
async function cancelEnrollment(id) {
  if (!confirm(`Cancel enrollment #${id}?`)) return;
  try { const result = await api(`/api/enrollments/${id}/cancel`, {method:"PUT"}); alert(result.message); await loadCourses(); await loadEnrollments(); }
  catch(err) { alert(err.message); }
}
async function payEnrollment(id) {
  const failed = confirm("Choose OK to simulate a successful payment. Choose Cancel to record a failed attempt.");
  try {
    const result = await api("/api/payments", {method:"POST", body:JSON.stringify({ enrollmentId:id, method:failed ? "Online" : "Fail" })});
    alert(result.message); await loadEnrollments();
  } catch(err) { alert(err.message); }
}
async function loadAdminStats() {
  try {
    const [courses, learners, enrollments, payments] = await Promise.all([
      api("/api/courses"), api("/api/learners"), api("/api/enrollments"), api("/api/payments")
    ]);
    $("adminStats").innerHTML = [
      ["Courses", courses.length], ["Students", learners.length],
      ["Active enrollments", enrollments.filter(e=>e.status==="Active").length],
      ["Successful payments", payments.filter(p=>p.status==="Successful").length]
    ].map(([label,value])=>`<div class="stat-card"><span>${label}</span><strong>${value}</strong></div>`).join("");
  } catch(err) { console.warn(err); }
}
async function loadReports() {
  try {
    const r = await api("/api/reports");
    $("reportContent").innerHTML = `
      <div class="report-card"><small>Expected fees · active enrollments</small><strong>${money(r.expectedFees)}</strong></div>
      <div class="report-card"><small>Total collected · simulated successful payments</small><strong>${money(r.totalCollected)}</strong></div>
      <div class="report-card"><small>Payment attempts</small><strong>${r.successfulPaymentAttempts} success / ${r.failedPaymentAttempts} failed</strong></div>
      ${r.occupancy.map(c=>`<div class="report-card"><small>${safe(c.courseCode)} · ${safe(c.title)}</small><strong>${c.occupancyPercentage}%</strong><small>${c.occupiedSeats}/${c.totalSeats} seats occupied · ${c.availableSeats} available</small></div>`).join("")}`;
  } catch(err) { $("reportContent").innerHTML = `<p class="message error">${safe(err.message)}</p>`; }
}
async function refreshCurrent() {
  await loadCourses(); await loadEnrollments();
  if(currentUser.role === "Admin") await loadAdminStats();
}
