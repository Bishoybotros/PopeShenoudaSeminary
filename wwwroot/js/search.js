function highlight(text, query) {
    if (!query) return text;
    const regex = new RegExp(`(${query})`, 'gi');
    return text.replace(regex, '<span class="highlight">$1</span>');
}

function normalizeArabic(text) {
    return text
        .replace(/[إأآا]/g, 'ا')
        .replace(/ى/g, 'ي')
        .replace(/ة/g, 'ه')
        .toLowerCase();
}

/* ========= بناء الكارد ========= */
function buildSubjectCard({ subject, department, level }, query) {
    return `
    <div class="col-md-6 col-lg-4 mb-3">
        <div class="card subject-card h-100">

            <div class="card-image-top">
                <img src="./assets/images/subjects/${subject.image}"
                     class="subject-image w-100"
                     alt="${subject.name}">
            </div>

            <div class="card-body">
                <h6 class="subjname">
                    مادة ${highlight(subject.name, query)}
                </h6>

                <p class="category mb-0">
                    <span>القسم:</span>
                    ${highlight(department, query)}
                </p>

                <div class="d-flex align-items-center justify-content-between">
                    <p class="grade mb-0">
                        <span>الفرقة:</span>
                        ${highlight(level, query)}
                    </p>

                    <div class="author">
                        <img src="./assets/images/avatars/avatar-2.png" alt="">
                        <h6 class="mb-0">لويس عبد المسيح</h6>
                    </div>
                </div>
            </div>

            <div class="card-footer d-flex justify-content-between align-items-center">
                <span class="download-count">
                    مرات التحميل: ${subject.downloads}
                </span>

                <a href="${subject.download}" class="btn btn-outline-saint" download>
                    <i class="bi bi-cloud-arrow-down"></i> تحميل
                </a>
            </div>
        </div>
    </div>`;
}

/* ========= البحث ========= */
const params = new URLSearchParams(window.location.search);
const query = params.get('q');
const resultsDiv = document.getElementById('results');

if (!query) {
    resultsDiv.innerHTML = '<p>لم يتم إدخال كلمة بحث</p>';
} else {
    fetch('assets/data/subject.json')
        .then(res => res.json())
        .then(data => {

            const normalizedQuery = normalizeArabic(query);
            let results = [];

            // level = الفرقة
            for (const level in data) {

                // department = القسم
                for (const department in data[level]) {

                    // subjects = المواد
                    data[level][department].forEach(subject => {

                        if (
                            normalizeArabic(level).includes(normalizedQuery) ||
                            normalizeArabic(department).includes(normalizedQuery) ||
                            normalizeArabic(subject.name).includes(normalizedQuery)
                        ) {
                            results.push({
                                level,
                                department,
                                subject
                            });
                        }

                    });
                }
            }

            if (results.length === 0) {
                resultsDiv.innerHTML = '<p>لا توجد نتائج</p>';
                return;
            }

            let html = '<div class="row">';
            results.forEach(item => {
                html += buildSubjectCard(item, query);
            });
            html += '</div>';

            resultsDiv.innerHTML = html;

            const searchInput = document.getElementById('search');
            if (searchInput) searchInput.value = query;
        })
        .catch(err => {
            console.error(err);
            resultsDiv.innerHTML = '<p>حدث خطأ أثناء تحميل البيانات</p>';
        });
}
